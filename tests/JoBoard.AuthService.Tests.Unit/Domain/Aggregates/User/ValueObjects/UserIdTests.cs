﻿using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class UserIdTests
{
    [Fact]
    public void Generate()
    {
        var newUserId = UserId.Generate();
        
        Assert.NotEqual(default, newUserId.Value);
    }
    
    [Fact]
    public void FromValue()
    {
        var newUserId = UserId.FromValue(Guid.NewGuid());
        
        Assert.NotEqual(default, newUserId.Value);
    }
    
    [Fact]
    public void FromValueInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = UserId.FromValue(Guid.Empty);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var value = Guid.NewGuid();
        var userId1 = UserId.FromValue(value);
        var userId2 = UserId.FromValue(value);
        
        Assert.Equal(userId1, userId2);
    }
}