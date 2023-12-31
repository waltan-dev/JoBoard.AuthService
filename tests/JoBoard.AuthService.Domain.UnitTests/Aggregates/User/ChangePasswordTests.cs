using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Domain.UnitTests.Builders;
using Moq;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ChangePasswordTests
{
    private static string GetNewPassword() => "newPassword";
    private static string GetNewPasswordHash() => "newPasswordHash";
    
    private static IPasswordHasher GetPasswordHasherStub()
    {
        var passwordHasherStub = new Mock<IPasswordHasher>();
        passwordHasherStub
            .Setup(x => x.Hash(GetNewPassword()))
            .Returns(GetNewPasswordHash()); // return new hash for new password
        passwordHasherStub
            .Setup(x => x.Verify(UserTestsHelper.DefaultPasswordHash, UserTestsHelper.DefaultPassword))
            .Returns(true); // verifies current password before change to new
        return passwordHasherStub.Object;
    }
    
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPassword = GetNewPassword();
        var newPasswordHash = GetNewPasswordHash();
        
        user.ChangePassword(UserTestsHelper.DefaultPassword, newPassword, passwordHasherStub);
        
        Assert.Equal(newPasswordHash, user.PasswordHash);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", "newPassword", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithExternalAccount().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", "newPassword", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<ArgumentException>(() =>
        {
            user.ChangePassword(" ", " ", passwordHasherStub);
        });
    }
}