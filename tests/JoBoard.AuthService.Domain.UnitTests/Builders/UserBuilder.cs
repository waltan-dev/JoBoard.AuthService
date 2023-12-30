using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Builders;

public class UserBuilder
{
    public UserId UserId = UserId.Generate();
    public FullName FullName = new("Ivan", "Ivanov");
    public Email Email = new("ivan@gmail.com");
    public AccountType AccountType = AccountType.Worker;
    public string PasswordHash = "hash";
    public ConfirmationToken ConfirmationToken = ConfirmationToken.Generate(expiresInHours: 24);
    
    public UserStatus UserStatus = UserStatus.Pending;

    public ExternalNetworkAccount? ExternalNetworkAccount = null;

    public User Build()
    {
        User user;

        // create new user by email and password
        if (ExternalNetworkAccount == null)
        {
            user = new User(
                userId: UserId,
                fullName: FullName,
                email: Email,
                accountType: AccountType,
                passwordHash: PasswordHash,
                confirmationToken: ConfirmationToken);
        }
        // create new user by external network account
        else
        {
            user = new User(
                userId: UserId,
                fullName: FullName,
                email: Email,
                accountType: AccountType,
                externalNetworkAccount: ExternalNetworkAccount,
                confirmationToken: ConfirmationToken);
        }
        
        if(UserStatus == UserStatus.Active)
            user.ConfirmEmail(ConfirmationToken.Value);
        
        return user;
    }
}