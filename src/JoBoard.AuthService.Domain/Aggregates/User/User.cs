using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Extensions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>, IAggregateRoot
{
    public DateTime RegisteredAt { get; init; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; private set; }
    public ConfirmationToken? RegisterConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    public ICollection<ExternalAccount> ExternalAccounts { get; private set; }
    public Email? NewEmail { get; private set; }
    public ConfirmationToken? NewEmailConfirmationToken { get; private set; }
    
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
        ExternalAccounts = new List<ExternalAccount>();
    }
    
    /// <summary>
    /// Register new user by google account
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, ExternalAccount externalAccount)
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
        ExternalAccounts = new List<ExternalAccount>() { externalAccount };
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
        
        ExternalAccounts.Add(externalAccount);
    }

    public void RequestPasswordReset(ConfirmationToken newToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (ResetPasswordConfirmToken != null && ResetPasswordConfirmToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Password reset has been requested already");
        
        ResetPasswordConfirmToken = newToken;
    }

    public void ResetPassword(string token, string newPassword, IPasswordHasher passwordHasher)
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
    }

    public void RequestEmailChange(Email newEmail, ConfirmationToken confirmationToken)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if(Email.Equals(newEmail))
            throw new DomainException("New email is the same as current");
        
        if (NewEmailConfirmationToken != null && NewEmailConfirmationToken.Expiration > DateTime.UtcNow)
            throw new DomainException("Email change has been requested already");

        NewEmail = newEmail;
        NewEmailConfirmationToken = confirmationToken;
    }

    public void ChangeEmail(string token)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        Guard.IsNotNullOrWhiteSpace(token);

        if (NewEmailConfirmationToken == null || NewEmail == null)
            throw new DomainException("Email change has not been requested");

        if (NewEmailConfirmationToken.Verify(token) == false)
            throw new DomainException("Invalid confirmation token");

        Email = NewEmail;
        EmailConfirmed = true;
        NewEmail = null;
        NewEmailConfirmationToken = null;
    }

    public void ChangeRole(UserRole newRole)
    {
        ThrowIfEmailIsNotConfirmed();
        ThrowIfBlockedOrDeactivated();
        
        if (newRole.Equals(UserRole.Hirer) == false && newRole.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");

        Role = newRole;
    }

    public void Deactivate()
    {
        ThrowIfBlockedOrDeactivated();
        
        Status = UserStatus.Deactivated;
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