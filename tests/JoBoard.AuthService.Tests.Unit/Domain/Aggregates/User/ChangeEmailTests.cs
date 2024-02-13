using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ChangeEmailTests
{
    private static Email CreateNewEmail() => new("newEmail@gmail.com");
    
    [Fact]
    public void RequestEmailChange()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedEmailChangeDomainEvent);
    }
    
    [Fact]
    public void RequestEmailChangeWithoutConfirmedEmail()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().Build();
        var newEmail = CreateNewEmail();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeWithInactiveStatus()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithInactiveStatus().Build();
        var newEmail = CreateNewEmail();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeWithOldValue()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(oldEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeTwiceBeforeExpiration()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, new ConfirmationTokenBuilder().BuildExpired());
        
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedEmailChangeDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailChange()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        user.ConfirmEmailChange(confirmationToken.Value);
        
        Assert.Equal(newEmail, user.Email);
        Assert.True(user.EmailConfirmed);
        Assert.Null(user.NewEmail);
        Assert.Null(user.ChangeEmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedEmailDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInvalidToken()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, confirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange("invalid-token");
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithExpiredToken()
    {
        var expiredConfirmationToken = new ConfirmationTokenBuilder().BuildExpired();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, expiredConfirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange(expiredConfirmationToken.Value);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithoutRequest()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange(confirmationToken.Value);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithoutConfirmedEmail()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
            user.ConfirmEmailChange(confirmationToken.Value);
        });
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInactiveStatus()
    {
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
            user.ConfirmEmailChange(confirmationToken.Value);
        });
    }
}