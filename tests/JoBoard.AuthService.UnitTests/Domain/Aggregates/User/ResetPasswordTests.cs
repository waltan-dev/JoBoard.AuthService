using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Moq;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ResetPasswordTests
{
    private const string NewPasswordHash = "newPasswordHash";
    
    [Fact]
    public void RequestResetPassword()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        
        user.RequestPasswordReset(confirmationToken);
        
        Assert.NotNull(user.ResetPasswordConfirmToken);
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void RequestResetPasswordTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
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
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
        user.RequestPasswordReset(expiredConfirmationToken);

        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void RequestResetPasswordWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();

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
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);
        
        user.ConfirmPasswordReset(confirmationToken.Value, "someNewPassword", passwordHasherStub);
        
        Assert.Equal(NewPasswordHash, user.PasswordHash);
        Assert.Null(user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void ResetPasswordWithInvalidToken()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("invalid-token", "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordAfterExpiration()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
        user.RequestPasswordReset(expiredConfirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset(expiredConfirmationToken.Value, "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordWithoutRequest()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("invalid-token", "newPasswordHash", passwordHasherStub);
        });
    }
    
    [Fact]
    public void ResetPasswordWithInactiveStatus()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            user.ConfirmPasswordReset(confirmationToken.Value, "someNewPassword", passwordHasherStub);
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