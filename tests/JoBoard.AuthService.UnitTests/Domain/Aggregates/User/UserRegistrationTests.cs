using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var newUser = new UserBuilder().Build();

        Assert.Equal(UserFixture.DefaultUserId, newUser.Id);
        Assert.Equal(UserFixture.DefaultFullName, newUser.FullName);
        Assert.Equal(UserFixture.DefaultEmail, newUser.Email);
        Assert.Equal(UserFixture.DefaultUserRole, newUser.Role);
        Assert.Equal(UserFixture.DefaultPasswordHash, newUser.PasswordHash);
        Assert.Equal(UserFixture.DefaultConfirmationToken, newUser.RegisterConfirmToken);
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

        Assert.Equal(UserFixture.DefaultUserId, newUser.Id);
        Assert.Equal(UserFixture.DefaultFullName, newUser.FullName);
        Assert.Equal(UserFixture.DefaultEmail, newUser.Email);
        Assert.Equal(UserFixture.DefaultUserRole, newUser.Role);
        Assert.Equal(UserFixture.DefaultConfirmationToken, newUser.RegisterConfirmToken);
        Assert.Equal(UserFixture.DefaultExternalAccount, newUser.ExternalAccounts.First());
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

        user.ConfirmEmail(UserFixture.DefaultConfirmationToken.Value);

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
        var user = new UserBuilder().WithExpiredRegisterToken().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail(user.RegisterConfirmToken!.Value);
        });
    }
}