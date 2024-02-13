using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity, IAggregateRoot
{
    public new UserId Id { get; init; }
    public DateTime RegisteredAt { get; init; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Email? NewEmail { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; private set; }
    
    private List<ExternalAccount> _externalAccounts;
    public IReadOnlyCollection<ExternalAccount> ExternalAccounts => _externalAccounts.AsReadOnly();
    
    public ConfirmationToken? RegisterConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    public ConfirmationToken? ChangeEmailConfirmToken { get; private set; }
    public ConfirmationToken? AccountDeactivationConfirmToken { get; private set; }
    
    private User() {} // only for ef core 
    
    /// <summary>
    /// Register new user by email and password
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, 
        string passwordHash, ConfirmationToken registerConfirmToken)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        Id = userId;
        RegisteredAt = DateTime.UtcNow.TrimMilliseconds();
        FullName = fullName;
        Email = email;
        EmailConfirmed = false;
        Role = role;
        Status = UserStatus.Pending;
        PasswordHash = passwordHash;
        RegisterConfirmToken = registerConfirmToken;
        _externalAccounts = new List<ExternalAccount>();

        AddDomainEvent(new UserRegisteredDomainEvent(this));
    }
    
    /// <summary>
    /// Register new user by google account
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, string googleUserId)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        Id = userId;
        RegisteredAt = DateTime.UtcNow.TrimMilliseconds();
        FullName = fullName;
        Email = email;
        EmailConfirmed = true;
        Role = role;
        Status = UserStatus.Active;
        _externalAccounts = new List<ExternalAccount> { new(googleUserId, ExternalAccountProvider.Google) };
        
        AddDomainEvent(new UserRegisteredDomainEvent(this));
    }
    
    public void ConfirmEmail(string token)
    {
        ThrowIfBlockedOrDeactivated();
        
        Guard.IsNotNullOrWhiteSpace(token);
        
        if(RegisterConfirmToken?.Verify(token) is null or false)
            throw new DomainException("Invalid token");
        
        EmailConfirmed = true;
        RegisterConfirmToken = null;
        if(Status.Equals(UserStatus.Pending))
            Status = UserStatus.Active;
        AddDomainEvent(new UserConfirmedEmailDomainEvent(this));
    }

    public void LoginWithPassword(string password, IPasswordHasher passwordHasher)
    {
        ThrowIfBlockedOrDeactivated();
        
        if(PasswordHash == null)
            throw new DomainException("Invalid email or password");
        
        var isValidPassword = passwordHasher.Verify(PasswordHash, password);
        if(isValidPassword == false)
            throw new DomainException("Invalid email or password");
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

    public void RequestPasswordReset(ConfirmationToken newToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ResetPasswordConfirmToken != null && ResetPasswordConfirmToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Password reset has been requested already");
        
        ResetPasswordConfirmToken = newToken;
        AddDomainEvent(new UserRequestedPasswordResetDomainEvent(this));
    }

    public void ConfirmPasswordReset(string token, string newPassword, IPasswordHasher passwordHasher)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        Guard.IsNotNullOrWhiteSpace(token);
        Guard.IsNotNullOrWhiteSpace(newPassword);
        
        if (DateTime.UtcNow > ResetPasswordConfirmToken?.Expiration)
            throw new DomainException("Confirmation token is expired");

        if(ResetPasswordConfirmToken?.Verify(token) is null or false)
            throw new DomainException("Invalid token");

        PasswordHash = passwordHasher.Hash(newPassword);
        ResetPasswordConfirmToken = null;
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher passwordHasher)
    {
        ThrowIfBlockedOrDeactivated();
        
        Guard.IsNotNullOrWhiteSpace(currentPassword);
        Guard.IsNotNullOrWhiteSpace(newPassword);
        
        if(PasswordHash == null)
            throw new DomainException("No current password");

        if (passwordHasher.Verify(PasswordHash, currentPassword) == false)
            throw new DomainException("Incorrect current password");
        
        PasswordHash = passwordHasher.Hash(newPassword);
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
        
        Guard.IsNotNullOrWhiteSpace(token);

        if (ChangeEmailConfirmToken == null || NewEmail == null)
            throw new DomainException("Email change has not been requested");

        if (ChangeEmailConfirmToken.Verify(token) == false)
            throw new DomainException("Invalid confirmation token");

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
        
        Guard.IsNotNullOrWhiteSpace(token);
        
        if (AccountDeactivationConfirmToken == null)
            throw new DomainException("Account deactivation has not been requested");

        if (AccountDeactivationConfirmToken.Verify(token) == false)
            throw new DomainException("Invalid confirmation token");

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