using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>, IAggregateRoot
{
    public DateTime RegisteredAt { get; }
    public FullName FullName { get; }
    public Email Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; private set; }
    public ConfirmationToken RegisterConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    private ICollection<ExternalNetworkAccount> _externalNetworkAccounts { get; }
    public IReadOnlyCollection<ExternalNetworkAccount> ExternalNetworkAccounts 
        => (IReadOnlyCollection<ExternalNetworkAccount>)_externalNetworkAccounts;
    public Email? NewEmail { get; private set; }
    public ConfirmationToken? NewEmailConfirmationToken { get; private set; }
    
    /// <summary>
    /// Register new user by email and password
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, 
        string passwordHash, ConfirmationToken registerConfirmToken)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        Id = userId;
        RegisteredAt = DateTime.UtcNow;
        FullName = fullName;
        Email = email;
        EmailConfirmed = false;
        Role = role;
        Status = UserStatus.Pending;
        PasswordHash = passwordHash;
        RegisterConfirmToken = registerConfirmToken;
        _externalNetworkAccounts = new List<ExternalNetworkAccount>();
    }
    
    /// <summary>
    /// Register new user by external network account
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, 
        ExternalNetworkAccount externalNetworkAccount, ConfirmationToken registerConfirmToken)
    {
        if (role.Equals(UserRole.Hirer) == false && role.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");
        
        Id = userId;
        RegisteredAt = DateTime.UtcNow;
        FullName = fullName;
        Email = email;
        EmailConfirmed = false;
        Role = role;
        Status = UserStatus.Pending;
        RegisterConfirmToken = registerConfirmToken;
        _externalNetworkAccounts = new List<ExternalNetworkAccount>() { externalNetworkAccount };
    }
    
    public void ConfirmEmail(string token, DateTime? dateTimeNow = null)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        
        dateTimeNow ??= DateTime.UtcNow;
        
        if(EmailConfirmed)
            return;
        
        if(RegisterConfirmToken?.Verify(token, dateTimeNow) is null or false)
            throw new DomainException("Invalid token");
        
        EmailConfirmed = true;
        RegisterConfirmToken = null;
        if(Status.Equals(UserStatus.Pending))
            Status = UserStatus.Active;
    }

    public void AttachNetwork(ExternalNetworkAccount externalAccount)
    {
        CheckStatus();
        
        if (_externalNetworkAccounts.Contains(externalAccount))
            return;
        
        _externalNetworkAccounts.Add(externalAccount);
    }

    public void RequestPasswordReset(ConfirmationToken newToken, DateTime? dateTimeNow = null)
    {
        CheckStatus();
        
        dateTimeNow ??= DateTime.UtcNow;
        
        if (ResetPasswordConfirmToken != null && ResetPasswordConfirmToken.Expiration > dateTimeNow)
            throw new DomainException("Password reset has been requested already");
        
        ResetPasswordConfirmToken = newToken;
    }

    public void ResetPassword(string token, string newPassword, IPasswordHasher passwordHasher, DateTime? dateTimeNow = null)
    {
        CheckStatus();
        
        Guard.IsNotNullOrWhiteSpace(token);
        Guard.IsNotNullOrWhiteSpace(newPassword);
        
        dateTimeNow ??= DateTime.UtcNow;
        
        if (dateTimeNow > ResetPasswordConfirmToken?.Expiration)
            throw new DomainException("Confirmation token is expired");

        if(ResetPasswordConfirmToken?.Verify(token, dateTimeNow) is null or false)
            throw new DomainException("Invalid token");

        PasswordHash = passwordHasher.Hash(newPassword);
        ResetPasswordConfirmToken = null;
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher passwordHasher)
    {
        CheckStatus();
        
        Guard.IsNotNullOrWhiteSpace(currentPassword);
        Guard.IsNotNullOrWhiteSpace(newPassword);
        
        if(PasswordHash == null)
            throw new DomainException("No current password");

        if (passwordHasher.Verify(PasswordHash, currentPassword) == false)
            throw new DomainException("Incorrect current password");
        
        PasswordHash = passwordHasher.Hash(newPassword);
    }

    public void RequestEmailChange(Email newEmail, ConfirmationToken confirmationToken, DateTime? dateTimeNow = null)
    {
        CheckStatus();
        
        if(Email.Equals(newEmail))
            throw new DomainException("New email is the same as current");

        dateTimeNow ??= DateTime.UtcNow;
        if (NewEmailConfirmationToken != null && NewEmailConfirmationToken.Expiration > dateTimeNow)
            throw new DomainException("Email change has been requested already");

        NewEmail = newEmail;
        NewEmailConfirmationToken = confirmationToken;
    }

    public void ChangeEmail(string token, DateTime? dateTimeNow = null)
    {
        CheckStatus();
        
        Guard.IsNotNullOrWhiteSpace(token);
        dateTimeNow ??= DateTime.UtcNow;

        if (NewEmailConfirmationToken == null || NewEmail == null)
            throw new DomainException("Email change has not been requested");

        if (NewEmailConfirmationToken.Verify(token, dateTimeNow) == false)
            throw new DomainException("Invalid confirmation token");

        Email = NewEmail;
        EmailConfirmed = true;
        NewEmail = null;
        NewEmailConfirmationToken = null;
    }

    public void ChangeRole(UserRole newRole)
    {
        CheckStatus();
        
        if (newRole.Equals(UserRole.Hirer) == false && newRole.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");

        Role = newRole;
    }

    public void Deactivate()
    {
        CheckStatus();
        
        Status = UserStatus.Deactivated;
    }
    
    public void CheckStatus()
    {
        if(Status.Equals(UserStatus.Pending))
            throw new DomainException("Email is not confirmed yet");
        if(Status.Equals(UserStatus.Deactivated))
            throw new DomainException("Your account has been deactivated");
        if(Status.Equals(UserStatus.Blocked))
            throw new DomainException("User is blocked");
    }
}