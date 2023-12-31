using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.UnitTests.Builders;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var newUser = new UserBuilder().Build();

        Assert.Equal(UserTestsHelper.DefaultUserId, newUser.Id);
        Assert.Equal(UserTestsHelper.DefaultFullName, newUser.FullName);
        Assert.Equal(UserTestsHelper.DefaultEmail, newUser.Email);
        Assert.Equal(UserTestsHelper.DefaultAccountType, newUser.AccountType);
        Assert.Equal(UserTestsHelper.DefaultPasswordHash, newUser.PasswordHash);
        Assert.Equal(UserTestsHelper.DefaultConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
    }
    
    [Fact]
    public void CreateNewUserByExternalNetworkAccount()
    {
        var newUser = new UserBuilder().WithExternalAccount().Build();

        Assert.Equal(UserTestsHelper.DefaultUserId, newUser.Id);
        Assert.Equal(UserTestsHelper.DefaultFullName, newUser.FullName);
        Assert.Equal(UserTestsHelper.DefaultEmail, newUser.Email);
        Assert.Equal(UserTestsHelper.DefaultAccountType, newUser.AccountType);
        Assert.Equal(UserTestsHelper.DefaultConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserTestsHelper.DefaultExternalNetworkAccount, newUser.ExternalNetworkAccounts.First());
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
    }

    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var userBuilder = new UserBuilder();
        var user = userBuilder.Build();

        user.ConfirmEmail(UserTestsHelper.DefaultConfirmationToken.Value);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var user = new UserBuilder().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail("invalid-token");
        });
    }
    
    [Fact]
    public void ConfirmEmailWithExpiredToken()
    {
        var userBuilder = new UserBuilder();
        var user = userBuilder.Build();

        Assert.Throws<DomainException>(() =>
        {
            var futureTime = DateTime.UtcNow.AddDays(2);
            user.ConfirmEmail(UserTestsHelper.DefaultConfirmationToken.Value, dateTimeNow: futureTime);
        });
    }
}