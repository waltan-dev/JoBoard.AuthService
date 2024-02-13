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
    public void CreateNewUserByGoogleAccount()
    {
        var newUser = new UserBuilder().WithGoogleAccount().Build();

        Assert.Equal(UserFixture.DefaultUserId, newUser.Id);
        Assert.Equal(UserFixture.DefaultFullName, newUser.FullName);
        Assert.Equal(UserFixture.DefaultEmail, newUser.Email);
        Assert.Equal(UserFixture.DefaultUserRole, newUser.Role);
        Assert.Null(newUser.RegisterConfirmToken);
        Assert.Equal(UserFixture.DefaultGoogleAccount, newUser.ExternalAccounts.First());
        Assert.Equal(UserStatus.Active, newUser.Status);
        Assert.True(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
    }
    
    [Fact]
    public void CreateNewUserByExternalAccountWithInvalidRole()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new UserBuilder().WithGoogleAccount().WithAdminRole().Build();
        });
    }
}