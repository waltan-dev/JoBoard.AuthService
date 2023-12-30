using JoBoard.AuthService.Domain.Common;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>
{
    public DateTime RegisteredAt { get; }
    public FullName FullName { get; }
    public Email Email { get; }
    public bool EmailConfirmed { get; private set; }
    public AccountType AccountType { get; }
    public UserStatus Status { get; private set; }
    public string? PasswordHash { get; }
    public ConfirmationToken? ConfirmationToken { get; }
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

    public void ConfirmEmail(string token)
    {
        if(EmailConfirmed)
            return;
        
        if(ConfirmationToken?.IsValid(token) is null or false)
            throw new DomainException("Invalid token");
        
        EmailConfirmed = true;
        if(Status == UserStatus.Pending)
            Status = UserStatus.Active;
    }

    public void AttachNetwork(ExternalNetworkAccount externalAccount)
    {
        if (_externalNetworkAccounts.Contains(externalAccount))
            return;
        
        _externalNetworkAccounts.Add(externalAccount);
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