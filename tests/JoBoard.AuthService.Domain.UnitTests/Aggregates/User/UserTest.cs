using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class UserTest
{
    [Fact]
    public void CreateNewUser()
    {
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var accountType = AccountType.Worker;
        var passwordHash = "hash";
        var confirmationToken = ConfirmationToken.Generate(24);
        
        var newUser = new Domain.Aggregates.User.User(
            userId: userId,
            fullName: fullName,
            email: email,
            accountType: accountType,
            passwordHash: passwordHash,
            confirmationToken: confirmationToken);
        
        Assert.Equal(userId, newUser.Id);
        Assert.Equal(fullName, newUser.FullName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(accountType, newUser.AccountType);
        Assert.Equal(passwordHash, newUser.PasswordHash);
        Assert.Equal(confirmationToken, newUser.ConfirmationToken);
    }
}