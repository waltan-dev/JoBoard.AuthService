using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class ChangePasswordTests
{
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.CreateNew();
        
        user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        Assert.Equal(newPasswordHash, user.Password);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var user = UnitTestsRegistry.UserBuilder
            .WithGoogleAccount("test")
            .WithActiveStatus()
            .Build();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(" ", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithInactiveStatus()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.CreateNew();
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        });
    }
}