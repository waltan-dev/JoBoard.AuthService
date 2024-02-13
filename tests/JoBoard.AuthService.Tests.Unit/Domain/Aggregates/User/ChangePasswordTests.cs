using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ChangePasswordTests
{
    [Fact]
    public void ChangePassword()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPassword = PasswordFixtures.CreateNew();
        
        user.ChangePassword(PasswordFixtures.DefaultPassword, newPassword, passwordHasher);
        Assert.Equal(newPassword, user.PasswordHash);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", PasswordFixtures.CreateNew(), passwordHasher);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithGoogleAccount().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", PasswordFixtures.CreateNew(), passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(" ", PasswordFixtures.CreateNew(), passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithInactiveStatus()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var newPassword = PasswordFixtures.CreateNew();
        var user = new UserBuilder().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword(PasswordFixtures.DefaultPassword, newPassword, passwordHasher);
        });
    }
}