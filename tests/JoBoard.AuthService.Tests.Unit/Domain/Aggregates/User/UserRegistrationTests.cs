using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class UserRegistrationTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var role = UserRole.Worker;
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        var newUser = AuthService.Domain.Aggregates.UserAggregate.User.RegisterByEmailAndPassword(
            userId: userId,
            fullName: fullName,
            email: email,
            role: role, 
            passwordHash: newPasswordHash);

        Assert.Equal(userId, newUser.Id);
        Assert.Equal(fullName, newUser.FullName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(role, newUser.Role);
        Assert.Equal(newPasswordHash, newUser.PasswordHash);
        
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);

        Assert.Single(newUser.DomainEvents, ev => ev is UserRegisteredDomainEvent);
    }
    
    [Fact]
    public void CreateNewUserByEmailWithInvalidRole()
    {
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var role = UserRole.Admin;
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            AuthService.Domain.Aggregates.UserAggregate.User.RegisterByEmailAndPassword(
                userId: userId,
                fullName: fullName,
                email: email,
                role: role, 
                passwordHash: newPasswordHash);
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
        
        var newUser = AuthService.Domain.Aggregates.UserAggregate.User.RegisterByGoogleAccount(
            userId: userId,
            fullName: fullName,
            email: email,
            role: role, 
            googleUserId: googleUserId);

        Assert.Equal(userId, newUser.Id);
        Assert.Equal(fullName, newUser.FullName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(role, newUser.Role);
        Assert.Equal(googleUserId, newUser.ExternalAccounts.First().Value.ExternalUserId);
        Assert.Equal(ExternalAccountProvider.Google, newUser.ExternalAccounts.First().Value.Provider);
        
        Assert.Single(newUser.ExternalAccounts);
        Assert.Null(newUser.EmailConfirmToken);
        Assert.Equal(UserStatus.Active, newUser.Status);
        Assert.True(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
        
        Assert.Single(newUser.DomainEvents, ev => ev is UserRegisteredDomainEvent);
    }
    
    [Fact]
    public void CreateNewUserByGoogleWithInvalidRole()
    {
        var userId = UserId.Generate();
        var fullName = new FullName("Ivan", "Ivanov");
        var email = new Email("ivan@gmail.com");
        var role = UserRole.Admin;
        var googleUserId = GoogleFixtures.UserProfileForNewUser.Id;
        
        Assert.Throws<DomainException>(() =>
        {
            AuthService.Domain.Aggregates.UserAggregate.User.RegisterByGoogleAccount(
                userId: userId,
                fullName: fullName,
                email: email,
                role: role, 
                googleUserId: googleUserId);
        });
    }
}