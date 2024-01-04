using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>
{
    public DateTime RegisteredAt { get; }
    public FullName FullName { get; }
    public Email Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; private set; }
    
    // field for email confirmation after registration and for password resetting
    public ConfirmationToken? ConfirmationToken { get; private set; }
    
    // fields for manage external accounts
    private ICollection<ExternalNetworkAccount> _externalNetworkAccounts { get; }
    public IReadOnlyCollection<ExternalNetworkAccount> ExternalNetworkAccounts 
        => (IReadOnlyCollection<ExternalNetworkAccount>)_externalNetworkAccounts;
    
    // temp fields for email changing
    public Email? NewEmail { get; private set; }
    public ConfirmationToken? NewEmailConfirmationToken { get; private set; }
    
    /// <summary>
    /// Register new user by email and password
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, 
        string passwordHash, ConfirmationToken confirmationToken)
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
        ConfirmationToken = confirmationToken;
        _externalNetworkAccounts = new List<ExternalNetworkAccount>();
    }
    
    /// <summary>
    /// Register new user by external network account
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, UserRole role, 
        ExternalNetworkAccount externalNetworkAccount, ConfirmationToken confirmationToken)
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
        ConfirmationToken = confirmationToken;
        _externalNetworkAccounts = new List<ExternalNetworkAccount>() { externalNetworkAccount };
    }
    
    public void ConfirmEmail(string token, DateTime? dateTimeNow = null)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        
        dateTimeNow ??= DateTime.UtcNow;
        
        if(EmailConfirmed)
            return;
        
        if(ConfirmationToken?.Verify(token, dateTimeNow) is null or false)
            throw new DomainException("Invalid token");
        
        EmailConfirmed = true;
        ConfirmationToken = null;
        if(Status == UserStatus.Pending)
            Status = UserStatus.Active;
    }

    public void AttachNetwork(ExternalNetworkAccount externalAccount)
    {
        if (_externalNetworkAccounts.Contains(externalAccount))
            return;
        
        _externalNetworkAccounts.Add(externalAccount);
    }

    public void RequestPasswordReset(ConfirmationToken newToken, DateTime? dateTimeNow = null)
    {
        dateTimeNow ??= DateTime.UtcNow;

        if (EmailConfirmed == false)
            throw new DomainException("Email is not confirmed");
        
        if (ConfirmationToken != null && ConfirmationToken.Expiration > dateTimeNow)
            throw new DomainException("Password reset has been requested already");
        
        ConfirmationToken = newToken;
    }

    public void ResetPassword(string token, string newPassword, IPasswordHasher passwordHasher, DateTime? dateTimeNow = null)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        Guard.IsNotNullOrWhiteSpace(newPassword);
        
        dateTimeNow ??= DateTime.UtcNow;
        
        if (dateTimeNow > ConfirmationToken?.Expiration)
            throw new DomainException("Confirmation token is expired");

        if(ConfirmationToken?.Verify(token, dateTimeNow) is null or false)
            throw new DomainException("Invalid token");

        PasswordHash = passwordHasher.Hash(newPassword);
        ConfirmationToken = null;
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher passwordHasher)
    {
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
        if (Status != UserStatus.Active)
            throw new DomainException("User is not active");
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
        if (newRole.Equals(UserRole.Hirer) == false && newRole.Equals(UserRole.Worker) == false)
            throw new DomainException("Invalid role");

        Role = newRole;
    }
    
    public void CheckStatus()
    {
        switch (Status)
        {
            case UserStatus.Blocked:
                throw new DomainException("User is blocked");
            case UserStatus.Deactivated:
                throw new DomainException("Your account has been deactivated");
            case UserStatus.Pending:
                //    throw new UserStatusException("Email is not confirmed yet");
                break;
        }
    }
}