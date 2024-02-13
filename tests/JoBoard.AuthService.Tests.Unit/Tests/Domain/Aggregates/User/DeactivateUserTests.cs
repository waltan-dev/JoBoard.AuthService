using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class DeactivateUserTests
{
    [Fact]
    public void RequestAccountDeactivation()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Active, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void RequestAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationWithInactiveStatus()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceBeforeExpiration()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceAfterExpiration()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive(), UnitTestsRegistry.CurrentDateTime);
        
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.FutureDateTime);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivation()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestAccountDeactivation(token, UnitTestsRegistry.CurrentDateTime);
        
        user.ConfirmAccountDeactivation(token.Value, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Null(user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Deactivated, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserDeactivatedDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationTwice()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestAccountDeactivation(token, UnitTestsRegistry.CurrentDateTime);
        user.ConfirmAccountDeactivation(token.Value, UnitTestsRegistry.CurrentDateTime);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(token.Value, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithInvalidToken()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var token = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestAccountDeactivation(token, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation("invalid-token", UnitTestsRegistry.CurrentDateTime);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithExpiredToken()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(confirmationToken.Value, UnitTestsRegistry.FutureDateTime);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithoutRequest()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
            user.ConfirmAccountDeactivation(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        });
    }
}