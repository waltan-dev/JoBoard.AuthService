using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using Moq;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ResetPasswordTests
{
    private const string NewPasswordHash = "newPasswordHash";
    
    [Fact]
    public void RequestResetPassword()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        
        user.RequestPasswordReset(confirmationToken);
        
        Assert.NotNull(user.ResetPasswordConfirmToken);
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void RequestResetPasswordTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        user.RequestPasswordReset(confirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestResetPasswordTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = UserFixture.CreateExpiredConfirmationToken();
        user.RequestPasswordReset(expiredConfirmationToken);

        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void RequestResetPasswordWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void ResetPassword()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        user.ResetPassword(confirmationToken.Value, "someNewPassword", passwordHasherStub);
        
        Assert.Equal(NewPasswordHash, user.PasswordHash);
        Assert.Null(user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void ResetPasswordWithInvalidToken()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ResetPassword("invalid-token", "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordAfterExpiration()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = UserFixture.CreateExpiredConfirmationToken();
        user.RequestPasswordReset(expiredConfirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ResetPassword(expiredConfirmationToken.Value, "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordWithoutRequest()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ResetPassword("invalid-token", "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordWithInactiveStatus()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().Build();
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            user.ResetPassword(confirmationToken.Value, "someNewPassword", passwordHasherStub);
        });
    }
    
    private static IPasswordHasher GetPasswordHasherStub()
    {
        var passwordHasherStub = new Mock<IPasswordHasher>();
        passwordHasherStub
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("newPasswordHash");
        passwordHasherStub
            .Setup(x => x.Verify("hash", It.IsAny<string>()))
            .Returns(true);
        return passwordHasherStub.Object;
    }
}