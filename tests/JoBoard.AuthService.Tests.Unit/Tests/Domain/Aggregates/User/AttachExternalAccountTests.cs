using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class AttachExternalAccountTests
{
    [Fact]
    public void AttachExternalAccount()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var externalAccountUniquenessChecker = UnitTestsRegistry.ExternalAccountUniquenessChecker;
        
        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);
        
        Assert.Single(user.ExternalAccounts);
        Assert.Equal(externalAccount, user.ExternalAccounts.First().Value);
        Assert.Single(user.DomainEvents, ev => ev is UserAttachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var externalAccountUniquenessChecker = UnitTestsRegistry.ExternalAccountUniquenessChecker;

        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);

        Assert.Throws<DomainException>(() =>
        {
            user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);
        });
    }
    
    [Fact]
    public void AttachExternalAccountWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var externalAccountUniquenessChecker = UnitTestsRegistry.ExternalAccountUniquenessChecker;

        Assert.Throws<DomainException>(() =>
        {
            var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
            user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);
        });
    }
    
    [Fact]
    public void DetachExternalAccount()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        var externalAccountUniquenessChecker = UnitTestsRegistry.ExternalAccountUniquenessChecker;
        user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);
        
        user.DetachExternalAccount(externalAccount);
        
        Assert.Empty(user.ExternalAccounts);
        Assert.Single(user.DomainEvents, ev => ev is UserDetachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void DetachExternalAccountTwice()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var externalAccount = new ExternalAccountValue("externalUserId", ExternalAccountProvider.Google);
        var externalAccountUniquenessChecker = UnitTestsRegistry.ExternalAccountUniquenessChecker;
        user.AttachExternalAccount(externalAccount, externalAccountUniquenessChecker);
        
        user.DetachExternalAccount(externalAccount);
        user.DetachExternalAccount(externalAccount);
        
        Assert.Empty(user.ExternalAccounts);
        Assert.Single(user.DomainEvents, ev => ev is UserDetachedExternalAccountDomainEvent);
    }
    
    [Fact]
    public void DetachExternalAccountWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder
            .WithGoogleAccount("test")
            .WithInactiveStatus()
            .Build();

        Assert.Throws<DomainException>(() =>
        {
            user.DetachExternalAccount(user.ExternalAccounts.First().Value);
        });
    }
}