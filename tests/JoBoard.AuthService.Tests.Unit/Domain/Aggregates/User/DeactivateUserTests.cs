using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class DeactivateUserTests
{
    [Fact]
    public void RequestAccountDeactivation()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Active, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void RequestAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationWithInactiveStatus()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceBeforeExpiration()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceAfterExpiration()
    {
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(new ConfirmationTokenBuilder().BuildExpired());
        
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivation()
    {
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = new ConfirmationTokenBuilder().BuildActive();
        user.RequestAccountDeactivation(token);
        
        user.ConfirmAccountDeactivation(token.Value);
        
        Assert.Null(user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Deactivated, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserDeactivatedDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationTwice()
    {
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = new ConfirmationTokenBuilder().BuildActive();
        user.RequestAccountDeactivation(token);
        user.ConfirmAccountDeactivation(token.Value);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(token.Value);
        });
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithInvalidToken()
    {
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = new ConfirmationTokenBuilder().BuildActive();
        user.RequestAccountDeactivation(token);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation("invalid-token");
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithExpiredToken()
    {
        var expiredConfirmationToken = new ConfirmationTokenBuilder().BuildExpired();
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(expiredConfirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(expiredConfirmationToken.Value);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithoutRequest()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(confirmationToken.Value);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = TestsRegistry.UserBuilder.Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
            user.ConfirmAccountDeactivation(confirmationToken.Value);
        });
    }
}