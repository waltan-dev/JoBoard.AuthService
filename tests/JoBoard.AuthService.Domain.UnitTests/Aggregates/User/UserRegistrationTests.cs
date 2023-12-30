using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.UnitTests.Builders;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var userBuilder = new UserBuilder();
        var newUser = userBuilder.BuildWithEmailAndPassword(UserStatus.Pending);

        Assert.Equal(userBuilder.UserId, newUser.Id);
        Assert.Equal(userBuilder.FullName, newUser.FullName);
        Assert.Equal(userBuilder.Email, newUser.Email);
        Assert.Equal(userBuilder.AccountType, newUser.AccountType);
        Assert.Equal(userBuilder.PasswordHash, newUser.PasswordHash);
        Assert.Equal(userBuilder.ConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
    }
    
    [Fact]
    public void CreateNewUserByExternalNetworkAccount()
    {
        var userBuilder = new UserBuilder();
        var newUser = userBuilder.BuildWithExternalAccount(UserStatus.Pending);

        Assert.Equal(userBuilder.UserId, newUser.Id);
        Assert.Equal(userBuilder.FullName, newUser.FullName);
        Assert.Equal(userBuilder.Email, newUser.Email);
        Assert.Equal(userBuilder.AccountType, newUser.AccountType);
        Assert.Equal(userBuilder.ConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
        Assert.Equal(userBuilder.ExternalNetworkAccount, newUser.ExternalNetworkAccounts.First());
    }

    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var userBuilder = new UserBuilder();
        var user = userBuilder.BuildWithEmailAndPassword(UserStatus.Pending);

        user.ConfirmEmail(userBuilder.ConfirmationToken.Value);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Pending);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail("invalid-token");
        });
    }
    
    [Fact]
    public void ConfirmEmailWithExpiredToken()
    {
        var expiredToken = new ConfirmationToken("valid-token", DateTime.UtcNow.AddHours(1));
        var userBuilder = new UserBuilder { ConfirmationToken = expiredToken };
        var user = userBuilder.BuildWithEmailAndPassword(UserStatus.Pending);

        Assert.Throws<DomainException>(() =>
        {
            var futureTime = DateTime.UtcNow.AddHours(2);
            user.ConfirmEmail(expiredToken.Value, dateTimeNow: futureTime);
        });
    }
}