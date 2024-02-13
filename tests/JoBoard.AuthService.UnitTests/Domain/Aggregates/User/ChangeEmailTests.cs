﻿using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ChangeEmailTests
{
    private static Email CreateNewEmail() => new("newEmail@gmail.com");
    
    [Fact]
    public void RequestEmailChange()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
    }
    
    [Fact]
    public void RequestEmailChangeIfUserIsNotActive()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().Build();
        var newEmail = CreateNewEmail();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeWithOldValue()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(oldEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestEmailChangeTwice()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
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
        user.RequestEmailChange(newEmail, ConfirmationTokenFixtures.CreateExpired());
        
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
    }
    
    [Fact]
    public void ConfirmEmailChange()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        user.ConfirmEmailChange(confirmationToken.Value);
        
        Assert.Equal(newEmail, user.Email);
        Assert.True(user.EmailConfirmed);
        Assert.Null(user.NewEmail);
        Assert.Null(user.ChangeEmailConfirmToken);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInvalidToken()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
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
        var expiredConfirmationToken = ConfirmationTokenFixtures.CreateExpired();
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
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var user = new UserBuilder().WithActiveStatus().Build();
        var oldEmail = user.Email;

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange(confirmationToken.Value);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInactiveStatus()
    {
        var confirmationToken = ConfirmationTokenFixtures.CreateNew();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
            user.ConfirmEmailChange(confirmationToken.Value);
        });
    }
}