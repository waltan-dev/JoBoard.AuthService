using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class CanLoginTests
{
    [Fact]
    public void CanLogin()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLogin();
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLogin();
        });
    }
    
    [Fact]
    public void CanLoginWithValidPassword()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        var password = PasswordFixtures.DefaultPassword;
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;

        var exception = Record.Exception(() =>
        {
            user.CanLoginWithPassword(password, passwordHasherStub);
        });
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidPassword()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithPassword("invalid-password", passwordHasherStub);
        });
    }
    
    [Fact]
    public void CanLoginWithValidExternalAccount()
    {
        var user = UnitTestsRegistry.UserBuilder
            .WithGoogleAccount("test")
            .Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLoginWithExternalAccount(user.ExternalAccounts.First().Value);
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidExternalAccount()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithExternalAccount(new ExternalAccountValue("invalid", ExternalAccountProvider.Google));
        });
    }
}