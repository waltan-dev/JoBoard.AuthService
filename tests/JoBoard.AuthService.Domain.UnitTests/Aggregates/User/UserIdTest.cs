﻿using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class UserIdTest
{
    [Fact]
    public void CreateValid()
    {
        var guid = Guid.NewGuid();
        var newUserId = new UserId(guid);
        
        Assert.Equal(guid, newUserId.Value);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new UserId(Guid.Empty);
        });
    }
    
    [Fact]
    public void Generate()
    {
        var newUserId = UserId.Generate();
        
        Assert.NotEqual(Guid.Empty, newUserId.Value);
        Assert.NotEmpty(newUserId.Value.ToString());
    }
}