using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>
{
    public DateTime RegisteredAt { get; }
    public FullName FullName { get; }
    public Email Email { get; }
    public bool EmailConfirmed { get; private set; }
    public AccountType AccountType { get; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; private set; }
    public ConfirmationToken? ConfirmationToken { get; private set; }
    private ICollection<ExternalNetworkAccount> _externalNetworkAccounts { get; }
    public IReadOnlyCollection<ExternalNetworkAccount> ExternalNetworkAccounts 
        => (IReadOnlyCollection<ExternalNetworkAccount>)_externalNetworkAccounts;
    
    /// <summary>
    /// Register new user by email and password
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, AccountType accountType, 
        string passwordHash, ConfirmationToken confirmationToken)
    {
        Id = userId;
        RegisteredAt = DateTime.UtcNow;
        FullName = fullName;
        Email = email;
        EmailConfirmed = false;
        AccountType = accountType;
        Status = UserStatus.Pending;
        PasswordHash = passwordHash;
        ConfirmationToken = confirmationToken;
        _externalNetworkAccounts = new List<ExternalNetworkAccount>();
    }
    
    /// <summary>
    /// Register new user by external network account
    /// </summary>
    public User(UserId userId, FullName fullName, Email email, AccountType accountType, 
        ExternalNetworkAccount externalNetworkAccount, ConfirmationToken confirmationToken)
    {
        Id = userId;
        RegisteredAt = DateTime.UtcNow;
        FullName = fullName;
        Email = email;
        EmailConfirmed = false;
        AccountType = accountType;
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