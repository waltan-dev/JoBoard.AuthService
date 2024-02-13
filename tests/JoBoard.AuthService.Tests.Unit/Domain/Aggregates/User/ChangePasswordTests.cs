using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.DataFixtures;

using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ChangePasswordTests
{
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        Assert.Equal(newPasswordHash, user.PasswordHash);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var user = new UserBuilder().WithGoogleAccount().WithActiveStatus().Build();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(" ", newPasswordHash, passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithInactiveStatus()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        var user = new UserBuilder().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(PasswordFixtures.DefaultPassword, newPasswordHash, passwordHasherStub);
        });
    }
}