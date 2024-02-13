using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var role = UserRole.Worker;
        var password = PasswordFixtures.CreateDefault();
        var registerConfirmToken = ConfirmationTokenFixtures.CreateNew();
        
        var newUser = AuthService.Domain.Aggregates.User.User.RegisterByEmailAndPassword(
            userId: userId,
            fullName: fullName,
            email: email,
            role: role, 
            password: password,
            registerConfirmToken: registerConfirmToken);

        Assert.Equal(userId, newUser.Id);
        Assert.Equal(fullName, newUser.FullName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(role, newUser.Role);
        Assert.Equal(password, newUser.Password);
        Assert.Equal(registerConfirmToken, newUser.RegisterConfirmToken);
        
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
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var role = UserRole.Worker;
        var googleUserId = GoogleFixtures.UserProfileForNewUser.Id;
        
        var newUser = AuthService.Domain.Aggregates.User.User.RegisterByGoogleAccount(
            userId: userId,
            fullName: fullName,
            email: email,
            role: role, 
            googleUserId: googleUserId);

        Assert.Equal(userId, newUser.Id);
        Assert.Equal(fullName, newUser.FullName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(role, newUser.Role);
        Assert.Equal(googleUserId, newUser.ExternalAccounts.First().ExternalUserId);
        Assert.Equal(ExternalAccountProvider.Google, newUser.ExternalAccounts.First().Provider);
        
        Assert.Single(newUser.ExternalAccounts);
        Assert.Null(newUser.RegisterConfirmToken);
        Assert.Equal(UserStatus.Active, newUser.Status);
        Assert.True(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.Password);
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