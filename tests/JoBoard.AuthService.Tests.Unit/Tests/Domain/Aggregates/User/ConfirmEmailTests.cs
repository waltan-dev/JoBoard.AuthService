using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class ConfirmEmailTests
{
    [Fact]
    public void RequestEmailConfirmation()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void RequestEmailConfirmationWithInactiveStatus()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailConfirmationTwiceBeforeExpiration()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailConfirmationTwiceAfterExpiration()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;
        user.RequestEmailConfirmation(UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive(), UnitTestsRegistry.CurrentDateTime);
        
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.FutureDateTime);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        user.RequestEmailConfirmation(UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive(), UnitTestsRegistry.CurrentDateTime);

        user.ConfirmEmail(user.EmailConfirmToken!.Value, UnitTestsRegistry.CurrentDateTime);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
        Assert.Single(user.DomainEvents, ev => ev is UserConfirmedEmailDomainEvent);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail("invalid-token", UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmEmailWithExpiredToken()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        user.RequestEmailConfirmation(UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive(), UnitTestsRegistry.CurrentDateTime);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail(user.EmailConfirmToken!.Value, UnitTestsRegistry.FutureDateTime);
        });
    }
    
    [Fact]
    public void ConfirmEmailWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive(), UnitTestsRegistry.CurrentDateTime);
            user.ConfirmEmail(user.EmailConfirmToken!.Value, UnitTestsRegistry.CurrentDateTime);
        });
    }
}