using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using Moq;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

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
            .Setup(x => x.Verify(UserBuilder.DefaultPasswordHash, UserBuilder.DefaultPassword))
            .Returns(true); // verifies current password before change to new
        return passwordHasherStub.Object;
    }
    
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPassword = GetNewPassword();
        
        user.ChangePassword(UserBuilder.DefaultPassword, newPassword, passwordHasherStub);
        
        var newPasswordHash = GetNewPasswordHash();
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
        var user = new UserBuilder().WithGoogleAccount().WithActiveStatus().Build();

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