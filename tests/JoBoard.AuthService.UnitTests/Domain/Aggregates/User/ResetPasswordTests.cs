using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ResetPasswordTests
{
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
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);

        var newPassword = PasswordFixtures.CreateNew();
        user.ConfirmPasswordReset(confirmationToken.Value, newPassword);
        
        Assert.Equal(newPassword, user.PasswordHash);
        Assert.Null(user.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public void ResetPasswordWithInvalidToken()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset("invalid-token", newPassword);
        });
    }
    
    [Fact]
    public void ResetPasswordAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
        user.RequestPasswordReset(expiredConfirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset(expiredConfirmationToken.Value, newPassword);
        });
    }
    
    [Fact]
    public void ResetPasswordWithoutRequest()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset("invalid-token", newPassword);
        });
    }
    
    [Fact]
    public void ResetPasswordWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset(confirmationToken.Value, newPassword);
        });
    }
}