using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ChangePasswordTests
{
    [Fact]
    public void ChangePassword()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPassword = PasswordFixtures.NewPassword;
        
        user.ChangePassword(PasswordFixtures.DefaultPassword, newPassword, passwordHasher);
        
        Assert.Equal(PasswordFixtures.NewPasswordHash, user.PasswordHash);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", "newPassword", passwordHasher);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithGoogleAccount().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", "newPassword", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = PasswordFixtures.GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<ArgumentException>(() =>
        {
            user.ChangePassword(" ", " ", passwordHasherStub);
        });
    }
}