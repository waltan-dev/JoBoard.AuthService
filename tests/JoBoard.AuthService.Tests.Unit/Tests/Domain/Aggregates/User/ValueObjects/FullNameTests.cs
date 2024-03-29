﻿using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User.ValueObjects;

public class FullNameTests
{
    [Fact]
    public void CreateValid()
    {
        var fullName = new FullName("Ivan", "Ivanov");
        
        Assert.Equal("Ivan", fullName.FirstName);
        Assert.Equal("Ivanov", fullName.LastName);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new FullName(" ", string.Empty);
        });
    }
    
    [Fact]
    public void NameToString()
    {
        var fullName = new FullName("Ivan", "Ivanov");
        
        Assert.Equal("Ivan Ivanov", fullName.ToString());
    }
    
    [Fact]
    public void Compare()
    {
        var fullName1 = new FullName("Ivan", "Ivanov");
        var fullName2 = new FullName("Ivan", "Ivanov");
        
        Assert.Equal(fullName1, fullName2);
    }
}