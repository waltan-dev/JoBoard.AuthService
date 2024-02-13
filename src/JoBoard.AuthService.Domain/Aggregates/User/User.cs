using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity, IAggregateRoot
{
    public new UserId Id { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Email? NewEmail { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public Password? Password { get; private set; }
    
    private List<ExternalAccount> _externalAccounts;
    public IReadOnlyCollection<ExternalAccount> ExternalAccounts => _externalAccounts.AsReadOnly();

    private List<RefreshToken> _refreshTokens;
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    
    public ConfirmationToken? RegisterConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    public ConfirmationToken? ChangeEmailConfirmToken { get; private set; }
    public ConfirmationToken? AccountDeactivationConfirmToken { get; private set; }
    
    private User() {} // only for ef core 
    
    // universal constructor for different scenarios, e.g. register by google account or register by email and password
    private User(UserId userId, FullName fullName, Email email, bool emailConfirmed, UserRole role, UserStatus status,
        Password? password, ConfirmationToken? registerConfirmToken, ExternalAccount? externalAccount)
    {
        Id = userId;
        RegisteredAt = DateTime.UtcNow.TrimMilliseconds();
        FullName = fullName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        Role = role;
        Status = status;
        Password = password;
        RegisterConfirmToken = registerConfirmToken;
        _externalAccounts = externalAccount == null 
            ? new List<ExternalAccount>() 
            : new List<ExternalAccount> { externalAccount };
        _refreshTokens = new List<RefreshToken>();
        
        AddDomainEvent(new UserRegisteredDomainEvent(this));
    }

    public static User RegisterByEmailAndPassword(UserId userId, FullName fullName, Email email, UserRole role, 
        Password password, ConfirmationToken registerConfirmToken)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        return new User(
            userId: userId,
            fullName: fullName,
            email: email,
            emailConfirmed: false,
            role: role,
            status: UserStatus.Pending, 
            password: password, 
            registerConfirmToken: registerConfirmToken,
            null);
    }
    
    public static User RegisterByGoogleAccount(UserId userId, FullName fullName, Email email, UserRole role, 
        string googleUserId)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        return new User(
            userId: userId,
            fullName: fullName,
            email: email,
            emailConfirmed: true,
            role: role,
            status: UserStatus.Active, 
            password: null, 
            registerConfirmToken: null,
            new ExternalAccount(googleUserId, ExternalAccountProvider.Google));
    }
    
    public RefreshToken RefreshToken(string currentToken, ISecureTokenizer secureTokenizer, int tokenExpiresInHours)
    {
        var currentUserRefreshToken = _refreshTokens.FirstOrDefault(x => x.Token == currentToken);
        if (currentUserRefreshToken == null)
            throw new DomainException("Invalid refresh token");
        
        _refreshTokens.Remove(currentUserRefreshToken);

        var newUserRefreshToken = Aggregates.User.RefreshToken.Create(Id, tokenExpiresInHours, secureTokenizer);
        _refreshTokens.Add(newUserRefreshToken);
        return newUserRefreshToken;
    }
    
    public void RemoveRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Remove(refreshToken);
    }
    
    public void ConfirmEmail(string token)
    {
        ThrowIfBlockedOrDeactivated();

        if (RegisterConfirmToken == null)
            throw new DomainException("Email confirmation has not been requested");
        
        RegisterConfirmToken.Verify(token);
        
        EmailConfirmed = true;
        RegisterConfirmToken = null;
        if(Status.Equals(UserStatus.Pending))
            Status = UserStatus.Active;
        AddDomainEvent(new UserConfirmedEmailDomainEvent(this));
    }

    public RefreshToken LoginWithPassword(
        string passwordForCheck, 
        IPasswordHasher passwordHasher, 
        ISecureTokenizer secureTokenizer,
        int tokenExpiresInHours)
    {
        ThrowIfBlockedOrDeactivated();
        
        if(Password == null || Password.Verify(passwordForCheck, passwordHasher) == false)
            throw new DomainException("Invalid email or password");
        
        var newUserRefreshToken = Aggregates.User.RefreshToken.Create(Id, tokenExpiresInHours, secureTokenizer);
        _refreshTokens.Add(newUserRefreshToken);
        return newUserRefreshToken;
    }
    
    public RefreshToken LoginWithExternalAccount(
        ISecureTokenizer secureTokenizer,
        int tokenExpiresInHours)
    {
        ThrowIfBlockedOrDeactivated();
        
        var newUserRefreshToken = Aggregates.User.RefreshToken.Create(Id, tokenExpiresInHours, secureTokenizer);
        _refreshTokens.Add(newUserRefreshToken);
        return newUserRefreshToken;
    }
    
    public void UpdateFullName(FullName fullName)
    {
        ThrowIfBlockedOrDeactivated();
        
        FullName = fullName;
    }

    public void AttachExternalAccount(ExternalAccount externalAccount)
    {
        ThrowIfBlockedOrDeactivated();
        
        if (ExternalAccounts.Contains(externalAccount))
            return;
        
        _externalAccounts.Add(externalAccount);
    }
    
    public void DetachExternalAccount(ExternalAccount externalAccount)
    {
        ThrowIfBlockedOrDeactivated();
        
        if (ExternalAccounts.Contains(externalAccount) == false)
            return;
        
        _externalAccounts.Remove(externalAccount);
    }

    public void RequestPasswordReset(ConfirmationToken confirmationToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ResetPasswordConfirmToken != null && ResetPasswordConfirmToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Password reset has been requested already");
        
        ResetPasswordConfirmToken = confirmationToken;
        AddDomainEvent(new UserRequestedPasswordResetDomainEvent(this));
    }

    public void ConfirmPasswordReset(string token, Password newPassword)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ResetPasswordConfirmToken == null)
            throw new DomainException("Password reset has not been requested");
        
        ResetPasswordConfirmToken.Verify(token);

        Password = newPassword;
        ResetPasswordConfirmToken = null;
    }

    public void ChangePassword(string currentPassword, Password newPassword, IPasswordHasher passwordHasher)
    {
        ThrowIfBlockedOrDeactivated();
        
        if(this.Password == null)
            throw new DomainException("No current password");

        if (this.Password.Verify(currentPassword, passwordHasher) == false)
            throw new DomainException("Incorrect current password");
        
        Password = newPassword;
        AddDomainEvent(new UserChangedPasswordDomainEvent(this));
    }

    public void RequestEmailChange(Email newEmail, ConfirmationToken confirmationToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if(Email.Equals(newEmail))
            throw new DomainException("New email is the same as current");
        
        if (ChangeEmailConfirmToken != null && ChangeEmailConfirmToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Email change has been requested already");

        NewEmail = newEmail;
        ChangeEmailConfirmToken = confirmationToken;
        AddDomainEvent(new UserRequestedEmailChangeDomainEvent(this));
    }

    public void ConfirmEmailChange(string token)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ChangeEmailConfirmToken == null || NewEmail == null)
            throw new DomainException("Email change has not been requested");

        ChangeEmailConfirmToken.Verify(token);

        Email = NewEmail;
        EmailConfirmed = true;
        NewEmail = null;
        ChangeEmailConfirmToken = null;
        AddDomainEvent(new UserChangedEmailDomainEvent(this));
    }

    public void ChangeRole(UserRole newRole)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (newRole.Equals(UserRole.Hirer) == false && newRole.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");

        Role = newRole;
        AddDomainEvent(new UserChangedRoleDomainEvent(this));
    }

    public void RequestAccountDeactivation(ConfirmationToken confirmationToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (AccountDeactivationConfirmToken != null && AccountDeactivationConfirmToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Account deactivation has been requested already");
        
        AccountDeactivationConfirmToken = confirmationToken;
        AddDomainEvent(new UserRequestedAccountDeactivationDomainEvent(this));
    }

    public void ConfirmAccountDeactivation(string token)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (AccountDeactivationConfirmToken == null)
            throw new DomainException("Account deactivation has not been requested");

        AccountDeactivationConfirmToken.Verify(token);

        Status = UserStatus.Deactivated;
        AccountDeactivationConfirmToken = null;
        AddDomainEvent(new UserDeactivatedDomainEvent(this));
    }

    private void ThrowIfEmailIsNotConfirmed()
    {
        if(Status.Equals(UserStatus.Pending))
            throw new DomainException("Email is not confirmed yet");
    }
    
    private void ThrowIfBlockedOrDeactivated()
    {
        if(Status.Equals(UserStatus.Deactivated))
            throw new DomainException("Your account has been deactivated");
        if(Status.Equals(UserStatus.Blocked))
            throw new DomainException("User is blocked");
    }
}