using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

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
        Assert.Equal(UserTestsHelper.DefaultUserRole, newUser.Role);
        Assert.Equal(UserTestsHelper.DefaultPasswordHash, newUser.PasswordHash);
        Assert.Equal(UserTestsHelper.DefaultConfirmationToken, newUser.RegisterConfirmToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
    }
    
    [Fact]
    public void CreateNewUserByEmailWithInvalidRole()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new UserBuilder().WithAdminRole().Build();
        });
    }
    
    [Fact]
    public void CreateNewUserByExternalAccount()
    {
        var newUser = new UserBuilder().WithExternalAccount().Build();

        Assert.Equal(UserTestsHelper.DefaultUserId, newUser.Id);
        Assert.Equal(UserTestsHelper.DefaultFullName, newUser.FullName);
        Assert.Equal(UserTestsHelper.DefaultEmail, newUser.Email);
        Assert.Equal(UserTestsHelper.DefaultUserRole, newUser.Role);
        Assert.Equal(UserTestsHelper.DefaultConfirmationToken, newUser.RegisterConfirmToken);
        Assert.Equal(UserTestsHelper.DefaultExternalAccount, newUser.ExternalAccounts.First());
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
    }
    
    [Fact]
    public void CreateNewUserByExternalAccountWithInvalidRole()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new UserBuilder().WithExternalAccount().WithAdminRole().Build();
        });
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