using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.DataFixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ChangePasswordTests
{
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = TestsRegistry.PasswordHasher;
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = new UserPasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        Assert.Equal(newPasswordHash, user.Password);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasherStub = TestsRegistry.PasswordHasher;
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = new UserPasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = TestsRegistry.PasswordHasher;
        var user = TestsRegistry.UserBuilder.WithGoogleAccount().WithActiveStatus().Build();
        var newPasswordHash = new UserPasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = TestsRegistry.PasswordHasher;
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = new UserPasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(" ", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithInactiveStatus()
    {
        var passwordHasherStub = TestsRegistry.PasswordHasher;
        var newPasswordHash = new UserPasswordBuilder().Create(PasswordFixtures.NewPassword);
        var user = TestsRegistry.UserBuilder.WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        });
    }
}