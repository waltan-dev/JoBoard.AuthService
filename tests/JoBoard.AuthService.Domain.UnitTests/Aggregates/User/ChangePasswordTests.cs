using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Domain.UnitTests.Builders;
using Moq;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ChangePasswordTests
{
    private const string ValidCurrentPassword = "validCurrentPassword";
    
    [Fact]
    public void ChangePassword()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        
        user.ChangePassword(ValidCurrentPassword, "newPassword", passwordHasherStub);
    }
    
    [Fact]
    public void ChangePasswordWithInvalidCurrent()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("invalidCurrentPassword", "newPassword", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithoutCurrent()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithExternalAccount(UserStatus.Active);

        Assert.Throws<DomainException>(() =>
        {
            user.ChangePassword("someCurrentPassword", "newPassword", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ChangePasswordWithEmpty()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);

        Assert.Throws<ArgumentException>(() =>
        {
            user.ChangePassword(" ", " ", passwordHasherStub);
        });
    }

    private static IPasswordHasher GetPasswordHasherStub()
    {
        var passwordHasherStub = new Mock<IPasswordHasher>();
        passwordHasherStub
            .Setup(x => x.Hash(ValidCurrentPassword))
            .Returns("hash");
        passwordHasherStub
            .Setup(x => x.Verify("hash", ValidCurrentPassword))
            .Returns(true);
        return passwordHasherStub.Object;
    }
}