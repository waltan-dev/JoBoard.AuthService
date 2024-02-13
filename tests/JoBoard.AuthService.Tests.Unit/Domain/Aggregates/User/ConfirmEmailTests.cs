using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ConfirmEmailTests
{
    [Fact]
    public void RequestEmailConfirmation()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().Build();
        
        user.RequestEmailConfirmation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void RequestEmailConfirmationWithInactiveStatus()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailConfirmation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailConfirmationTwiceBeforeExpiration()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
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
        user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateExpired());
        
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestEmailConfirmation(confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(confirmationToken, user.EmailConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedEmailConfirmationDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var user = new UserBuilder().Build();
        user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateNew());

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
        user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateExpired());

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
            user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateNew());
            user.ConfirmEmail(user.EmailConfirmToken!.Value);
        });
    }
}