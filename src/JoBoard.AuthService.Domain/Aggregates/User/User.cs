using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
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
    public PasswordHash? PasswordHash { get; private set; }
    
    private List<ExternalAccount> _externalAccounts;
    public IReadOnlyCollection<ExternalAccount> ExternalAccounts => _externalAccounts.AsReadOnly();
    
    public ConfirmationToken? RegisterConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    public ConfirmationToken? ChangeEmailConfirmToken { get; private set; }
    public ConfirmationToken? AccountDeactivationConfirmToken { get; private set; }
    
    private User() {} // only for ef core 
    
    // universal constructor for different scenarios, e.g. register by google account or register by email and password
    private User(UserId userId, FullName fullName, Email email, bool emailConfirmed, UserRole role, UserStatus status,
        PasswordHash? passwordHash, ConfirmationToken? registerConfirmToken, ExternalAccountValue? externalAccountValue)
    {
        Id = userId;
        RegisteredAt = DateTime.UtcNow.TrimMilliseconds();
        FullName = fullName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        Role = role;
        Status = status;
        PasswordHash = passwordHash;
        RegisterConfirmToken = registerConfirmToken;
        _externalAccounts = externalAccountValue == null 
            ? new List<ExternalAccount>() 
            : new List<ExternalAccount> { new(Id, externalAccountValue) };
        
        AddDomainEvent(new UserRegisteredDomainEvent(this));
    }

    public static User RegisterByEmailAndPassword(UserId userId, FullName fullName, Email email, UserRole role, 
        PasswordHash passwordHash, ConfirmationToken registerConfirmToken)
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
            passwordHash: passwordHash, 
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
            passwordHash: null, 
            registerConfirmToken: null,
            externalAccountValue: new ExternalAccountValue(googleUserId, ExternalAccountProvider.Google));
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
    
    public void UpdateFullName(FullName fullName)
    {
        ThrowIfBlockedOrDeactivated();
        
        FullName = fullName;
        AddDomainEvent(new UserUpdatedNameDomainEvent(this));
    }

    public void AttachExternalAccount(ExternalAccountValue value)
    {
        ThrowIfBlockedOrDeactivated();
        
        if (ExternalAccounts.Select(x=>x.Value).Contains(value))
            return;
        
        _externalAccounts.Add(new ExternalAccount(Id, value));
        AddDomainEvent(new UserAttachedExternalAccountDomainEvent(this));
    }
    
    public void DetachExternalAccount(ExternalAccountValue value)
    {
        ThrowIfBlockedOrDeactivated();

        var extAcc = ExternalAccounts.FirstOrDefault(x => x.Value.Equals(value));
        if (extAcc == null)
            return;
        
        _externalAccounts.Remove(extAcc);
        AddDomainEvent(new UserDetachedExternalAccountDomainEvent(this));
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

    public void ConfirmPasswordReset(string token, PasswordHash newPasswordHash)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ResetPasswordConfirmToken == null)
            throw new DomainException("Password reset has not been requested");
        
        ResetPasswordConfirmToken.Verify(token);

        PasswordHash = newPasswordHash;
        ResetPasswordConfirmToken = null;
        AddDomainEvent(new UserChangedPasswordDomainEvent(this));
    }

    public void ChangePassword(string currentPassword, PasswordHash newPasswordHash, IPasswordHasher passwordHasher)
    {
        ThrowIfBlockedOrDeactivated();
        
        if(this.PasswordHash == null)
            throw new DomainException("No current password");

        if (this.PasswordHash.Verify(currentPassword, passwordHasher) == false)
            throw new DomainException("Incorrect current password");
        
        PasswordHash = newPasswordHash;
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
    
    public void Block()
    {
        Status = UserStatus.Blocked;
        AddDomainEvent(new UserBlockedDomainEvent(this));
    }

    public void CanLogin()
    {
        ThrowIfBlockedOrDeactivated();
    }
    
    public void CanLoginWithPassword(string password, IPasswordHasher passwordHasher)
    {
        ThrowIfBlockedOrDeactivated();

        if (PasswordHash == null || PasswordHash.Verify(password, passwordHasher) == false)
            throw new DomainException("Invalid email or password");
    }

    public void CanLoginWithExternalAccount(ValueObjects.ExternalAccountValue externalAccountValue)
    {
        ThrowIfBlockedOrDeactivated();

        if (_externalAccounts.Select(x=>x.Value).Contains(externalAccountValue) == false)
            throw new DomainException("Unknown external account");
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