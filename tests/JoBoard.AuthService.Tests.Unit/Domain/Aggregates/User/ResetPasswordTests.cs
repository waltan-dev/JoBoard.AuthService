using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ResetPasswordTests
{
    [Fact]
    public void RequestPasswordReset()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        
        user.RequestPasswordReset(confirmationToken);
        
        Assert.NotNull(user.ResetPasswordConfirmToken);
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetTwiceBeforeExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestPasswordReset(confirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestPasswordResetTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = new ConfirmationTokenBuilder().BuildExpired();
        user.RequestPasswordReset(expiredConfirmationToken);

        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetWithoutConfirmedEmail()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestPasswordResetWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void ConfirmPasswordReset()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestPasswordReset(confirmationToken);

        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash);
        
        Assert.Equal(newPasswordHash, user.PasswordHash);
        Assert.Null(user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ConfirmPasswordResetWithInvalidToken()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestPasswordReset(confirmationToken);
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("invalid-token", newPasswordHash);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = new ConfirmationTokenBuilder().BuildExpired();
        user.RequestPasswordReset(expiredConfirmationToken);
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset(expiredConfirmationToken.Value, newPasswordHash);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithoutRequest()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("some-token", newPasswordHash);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithoutConfirmedEmail()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newPasswordHash = new PasswordBuilder().Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash);
        });
    }
}