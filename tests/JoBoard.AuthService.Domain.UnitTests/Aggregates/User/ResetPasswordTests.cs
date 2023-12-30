using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Domain.UnitTests.Builders;
using Moq;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ResetPasswordTests
{
    private const string NewPasswordHash = "newPasswordHash";
    
    [Fact]
    public void RequestResetPassword()
    {
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
        
        user.RequestPasswordReset(confirmationToken);
        
        Assert.NotNull(user.ConfirmationToken);
        Assert.Equal(confirmationToken, user.ConfirmationToken);
    }
    
    [Fact]
    public void RequestResetPasswordTwice()
    {
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
        user.RequestPasswordReset(confirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestResetPasswordTwiceAfterExpiration()
    {
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        var futureTime = DateTime.UtcNow.AddHours(2);
        user.RequestPasswordReset(confirmationToken, dateTimeNow: futureTime);
        
        Assert.Equal(confirmationToken, user.ConfirmationToken);
    }
    
    [Fact]
    public void RequestResetPasswordWithoutEmailConfirmation()
    {
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Pending);
        var confirmationToken = CreateConfirmationToken();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void ResetPassword()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        user.ResetPassword(confirmationToken.Value, "someNewPassword", passwordHasherStub);
        
        Assert.Equal(NewPasswordHash, user.PasswordHash);
        Assert.Null(user.ConfirmationToken);
    }
    
    [Fact]
    public void ResetPasswordWithInvalidToken()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
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
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        var confirmationToken = CreateConfirmationToken();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            var futureDate = DateTime.UtcNow.AddHours(2);
            user.ResetPassword(confirmationToken.Value, "newPasswordHash", passwordHasherStub, futureDate);
        });
    }
    
    [Fact]
    public void ResetPasswordWithoutRequest()
    {
        var passwordHasherStub = GetPasswordHasherStub();
        var user = new UserBuilder().BuildWithEmailAndPassword(UserStatus.Active);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ResetPassword("invalid-token", "newPasswordHash", passwordHasherStub);
        });
    }
    
    private static ConfirmationToken CreateConfirmationToken()
    {
        return new ConfirmationToken("valid-token", DateTime.UtcNow.AddHours(1));
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