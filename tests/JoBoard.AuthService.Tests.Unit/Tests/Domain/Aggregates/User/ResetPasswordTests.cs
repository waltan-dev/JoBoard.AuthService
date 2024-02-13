using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class ResetPasswordTests
{
    [Fact]
    public void RequestPasswordReset()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.NotNull(user.ResetPasswordConfirmToken);
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetTwiceBeforeExpiration()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestPasswordResetTwiceAfterExpiration()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);

        confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.FutureDateTime);
        
        Assert.Equal(confirmationToken, user.ResetPasswordConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedPasswordResetDomainEvent);
    }
    
    [Fact]
    public void RequestPasswordResetWithoutConfirmedEmail()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestPasswordResetWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmPasswordReset()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);

        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Equal(newPasswordHash, user.Password);
        Assert.Null(user.ResetPasswordConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedPasswordDomainEvent);
    }
    
    [Fact]
    public void ConfirmPasswordResetWithInvalidToken()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("invalid-token", newPasswordHash, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetAfterExpiration()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash, UnitTestsRegistry.FutureDateTime);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithoutRequest()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmPasswordReset("some-token", newPasswordHash, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithoutConfirmedEmail()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
            user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmPasswordResetWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newPasswordHash = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.NewPassword);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestPasswordReset(confirmationToken, UnitTestsRegistry.CurrentDateTime);
            user.ConfirmPasswordReset(confirmationToken.Value, newPasswordHash, UnitTestsRegistry.CurrentDateTime);
        });
    }
}