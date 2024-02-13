using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ConfirmEmailTests
{
    [Fact]
    public void RequestEmailConfirmation()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().Build();
        
        user.RequestEmailConfirmation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void RequestEmailConfirmationWithInactiveStatus()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailConfirmationTwiceBeforeExpiration()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailConfirmation(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailConfirmationTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        user.RequestEmailConfirmation(new ConfirmationTokenBuilder().BuildExpired());
        
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestEmailConfirmation(confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var user = new UserBuilder().Build();
        user.RequestEmailConfirmation(new ConfirmationTokenBuilder().BuildActive());

        user.ConfirmEmail(user.EmailConfirmToken!.Value);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
        Assert.Single(user.DomainEvents, ev => ev is UserConfirmedEmailDomainEvent);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var user = new UserBuilder().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail("invalid-token");
        });
    }
    
    [Fact]
    public void ConfirmEmailWithExpiredToken()
    {
        var user = new UserBuilder().Build();
        user.RequestEmailConfirmation(new ConfirmationTokenBuilder().BuildExpired());

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail(user.EmailConfirmToken!.Value);
        });
    }
    
    [Fact]
    public void ConfirmEmailWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(new ConfirmationTokenBuilder().BuildActive());
            user.ConfirmEmail(user.EmailConfirmToken!.Value);
        });
    }
}