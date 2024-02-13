using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using ExternalAccountValue = JoBoard.AuthService.Domain.Aggregates.User.ValueObjects.ExternalAccountValue;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class AttachExternalAccountTests
{
    [Fact]
    public void AttachExternalAccount()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Single(user.ExternalAccounts);
        Assert.Equal(externalAccount, user.ExternalAccounts.First().Value);
        Assert.Single(user.DomainEvents, ev => ev is UserAttachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Single(user.ExternalAccounts);
        Assert.Equal(externalAccount, user.ExternalAccounts.First().Value);
        Assert.Single(user.DomainEvents, ev => ev is UserAttachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void AttachExternalAccountWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
            user.AttachExternalAccount(externalAccount);
        });
    }
    
    [Fact]
    public void DetachExternalAccount()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        
        user.DetachExternalAccount(externalAccount);
        
        Assert.Empty(user.ExternalAccounts);
        Assert.Single(user.DomainEvents, ev => ev is UserDetachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void DetachExternalAccountTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        
        user.DetachExternalAccount(externalAccount);
        user.DetachExternalAccount(externalAccount);
        
        Assert.Empty(user.ExternalAccounts);
        Assert.Single(user.DomainEvents, ev => ev is UserDetachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void DetachExternalAccountWithInactiveStatus()
    {
        var user = new UserBuilder().WithGoogleAccount().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.DetachExternalAccount(user.ExternalAccounts.First().Value);
        });
    }
}