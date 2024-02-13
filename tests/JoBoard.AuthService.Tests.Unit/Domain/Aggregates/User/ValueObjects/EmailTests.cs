﻿using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class EmailTests
{
    [Fact]
    public void CreateValidEmail()
    {
        var email = new Email("TeSt@GmAiL.cOm");
        
        Assert.Equal("test@gmail.com", email.Value);
    }

    [Fact]
    public void CreateInvalidEmail()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new Email("invalid-email");
        });
    }

    [Fact]
    public void CreateEmptyEmail()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new Email(string.Empty);
        });
    }
}