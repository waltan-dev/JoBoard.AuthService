using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ResetPasswordTests
{
    [Fact]
    public void RequestPasswordReset()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        
        user.RequestPasswordReset(confirmationToken);
        
        Assert.NotNull(user.ResetPasswordConfirmToken);
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetTwiceBeforeExpiration()
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
    public void RequestPasswordResetTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
        user.RequestPasswordReset(expiredConfirmationToken);

        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);
        
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetWithoutConfirmedEmail()
    {
        var user = new UserBuilder().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestPasswordResetWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
        });
    }
    
    [Fact]
    public void ConfirmPasswordReset()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestPasswordReset(confirmationToken);

        var newPassword = PasswordFixtures.CreateNew();
        user.ConfirmPasswordReset(confirmationToken.Value, newPassword);
        
        Assert.Equal(newPassword, user.PasswordHash);
        Assert.Null(user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ConfirmPasswordResetWithInvalidToken()
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
    public void ConfirmPasswordResetAfterExpiration()
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
    public void ConfirmPasswordResetWithoutRequest()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset("some-token", newPassword);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithoutConfirmedEmail()
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
    
    [Fact]
    public void ConfirmPasswordResetWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken);
            var newPassword = PasswordFixtures.CreateNew();
            user.ConfirmPasswordReset(confirmationToken.Value, newPassword);
        });
    }
}