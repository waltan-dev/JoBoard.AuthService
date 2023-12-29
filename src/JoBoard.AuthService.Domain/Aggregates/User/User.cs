using JoBoard.AuthService.Domain.Common;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class User : Entity<UserId>
{
    public DateTime RegisteredAt { get; }
    public FullName FullName { get; }
    public Email Email { get; }
    public bool EmailConfirmed { get; }
    public AccountType AccountType { get; }
    public UserStatus Status { get; }
    
    public string PasswordHash { get; }
    public ConfirmationToken? ConfirmationToken { get; }

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
    }
    
    public void CheckStatus()
    {
        switch (Status)
        {
            case UserStatus.Blocked:
                throw new UserStatusException("User is blocked");
            case UserStatus.Deactivated:
                throw new UserStatusException("Your account has been deactivated");
            case UserStatus.Pending:
                //    throw new UserStatusException("Email is not confirmed yet");
                break;
        }
    }
}