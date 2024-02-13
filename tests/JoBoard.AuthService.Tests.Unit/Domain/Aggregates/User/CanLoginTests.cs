﻿using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class CanLoginTests
{
    [Fact]
    public void CanLogin()
    {
        var user = new UserBuilder().Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLogin();
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLogin();
        });
    }
    
    [Fact]
    public void CanLoginWithValidPassword()
    {
        var user = new UserBuilder().Build();
        var password = PasswordFixtures.DefaultPassword;
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();

        var exception = Record.Exception(() =>
        {
            user.CanLoginWithPassword(password, passwordHasher);
        });
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidPassword()
    {
        var user = new UserBuilder().Build();
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithPassword("invalid-password", passwordHasher);
        });
    }
    
    [Fact]
    public void CanLoginWithValidExternalAccount()
    {
        var user = new UserBuilder().WithGoogleAccount().Build();
        
        var exception = Record.Exception(() =>
        {
            user.CanLoginWithExternalAccount(user.ExternalAccounts.First().Value);
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CanLoginWithInvalidExternalAccount()
    {
        var user = new UserBuilder().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.CanLoginWithExternalAccount(new ExternalAccountValue("invalid", ExternalAccountProvider.Google));
        });
    }
}