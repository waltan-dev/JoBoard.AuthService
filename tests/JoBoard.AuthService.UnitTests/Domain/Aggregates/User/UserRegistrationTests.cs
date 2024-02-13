using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var newUser = new UserBuilder().Build();

        Assert.Equal(UserBuilder.DefaultUserId, newUser.Id);
        Assert.Equal(UserBuilder.DefaultFullName, newUser.FullName);
        Assert.Equal(UserBuilder.DefaultEmail, newUser.Email);
        Assert.Equal(UserBuilder.DefaultUserRole, newUser.Role);
        Assert.Equal(UserBuilder.DefaultPasswordHash, newUser.PasswordHash);
        Assert.Equal(UserBuilder.DefaultConfirmationToken, newUser.RegisterConfirmToken);
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

        Assert.Equal(UserBuilder.DefaultUserId, newUser.Id);
        Assert.Equal(UserBuilder.DefaultFullName, newUser.FullName);
        Assert.Equal(UserBuilder.DefaultEmail, newUser.Email);
        Assert.Equal(UserBuilder.DefaultUserRole, newUser.Role);
        Assert.Null(newUser.RegisterConfirmToken);
        Assert.Equal(UserBuilder.DefaultGoogleAccount, newUser.ExternalAccounts.First());
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