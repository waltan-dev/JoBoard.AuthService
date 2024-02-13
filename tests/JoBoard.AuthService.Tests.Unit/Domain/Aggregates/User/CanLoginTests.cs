using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.DataFixtures;

using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class CanLoginTests
{
    [Fact]
    public void CanLogin()
    {
        var user = TestsRegistry.UserBuilder.Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLogin();
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInactiveStatus()
    {
        var user = TestsRegistry.UserBuilder.WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLogin();
        });
    }
    
    [Fact]
    public void CanLoginWithValidPassword()
    {
        var user = TestsRegistry.UserBuilder.Build();
        var password = PasswordFixtures.DefaultPassword;
        var passwordHasherStub = TestsRegistry.PasswordHasher;

        var exception = Record.Exception(() =>
        {
            user.CanLoginWithPassword(password, passwordHasherStub);
        });
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidPassword()
    {
        var user = TestsRegistry.UserBuilder.Build();
        var passwordHasherStub = TestsRegistry.PasswordHasher;

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithPassword("invalid-password", passwordHasherStub);
        });
    }
    
    [Fact]
    public void CanLoginWithValidExternalAccount()
    {
        var user = TestsRegistry.UserBuilder.WithGoogleAccount().Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLoginWithExternalAccount(user.ExternalAccounts.First().Value);
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidExternalAccount()
    {
        var user = TestsRegistry.UserBuilder.Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithExternalAccount(new ExternalAccountValue("invalid", ExternalAccountProvider.Google));
        });
    }
}