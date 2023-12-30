using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Builders;

public class UserBuilder
{
    public readonly UserId UserId = UserId.Generate();
    public readonly FullName FullName = new("Ivan", "Ivanov");
    public readonly Email Email = new("ivan@gmail.com");
    public readonly AccountType AccountType = AccountType.Worker;
    public readonly string PasswordHash = "hash";
    public ConfirmationToken ConfirmationToken = new("valid-token", DateTime.UtcNow.AddHours(24));
    
    public readonly ExternalNetworkAccount ExternalNetworkAccount = new("externalUserId", ExternalNetwork.Google);
    
    public User BuildWithEmailAndPassword(UserStatus userStatus)
    {
        var user = new User(
            userId: UserId,
            fullName: FullName,
            email: Email,
            accountType: AccountType,
            passwordHash: PasswordHash,
            confirmationToken: ConfirmationToken);
        
        if(userStatus == UserStatus.Active)
            user.ConfirmEmail(ConfirmationToken.Value);

        return user;
    }

    public User BuildWithExternalAccount(UserStatus userStatus)
    {
        var user = new User(
            userId: UserId,
            fullName: FullName,
            email: Email,
            accountType: AccountType,
            externalNetworkAccount: ExternalNetworkAccount,
            confirmationToken: ConfirmationToken);
        
        if(userStatus == UserStatus.Active)
            user.ConfirmEmail(ConfirmationToken.Value);
        
        return user;
    }
}