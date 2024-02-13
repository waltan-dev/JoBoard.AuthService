using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class DeactivateUserTests
{
    [Fact]
    public void RequestAccountDeactivation()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Active, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void RequestAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationWithInactiveStatus()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceBeforeExpiration()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
        });
    }
    
    [Fact]
    public void RequestAccountDeactivationTwiceAfterExpiration()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestAccountDeactivation(ConfirmationTokenFixtures.CreateExpired());
        
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestAccountDeactivation(confirmationToken);
        
        Assert.Equal(confirmationToken, user.AccountDeactivationConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedAccountDeactivationDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivation()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var token = ConfirmationTokenFixtures.CreateNew();
        user.RequestAccountDeactivation(token);
        
        user.ConfirmAccountDeactivation(token.Value);
        
        Assert.Null(user.AccountDeactivationConfirmToken);
        Assert.Equal(UserStatus.Deactivated, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserDeactivatedDomainEvent);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithInvalidToken()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var token = ConfirmationTokenFixtures.CreateNew();
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
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
        var user = new UserBuilder().WithActiveStatus().Build();
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
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmAccountDeactivation(confirmationToken.Value);
        });
        Assert.Equal(UserStatus.Active, user.Status);
    }
    
    [Fact]
    public void ConfirmAccountDeactivationWithoutConfirmedEmail()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestAccountDeactivation(confirmationToken);
            user.ConfirmAccountDeactivation(confirmationToken.Value);
        });
    }
}