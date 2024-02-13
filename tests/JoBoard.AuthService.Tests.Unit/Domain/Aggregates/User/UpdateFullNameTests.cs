﻿using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class UpdateFullNameTests
{
    [Fact]
    public void UpdateFullName()
    {
        var user = TestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newName = new FullName("New", "Name");

        user.UpdateFullName(newName);
        
        Assert.Equal(newName, user.FullName);
        Assert.Single(user.DomainEvents, ev => ev is UserUpdatedNameDomainEvent);
    }
    
    [Fact]
    public void UpdateFullNameWithInactiveStatus()
    {
        var user = TestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var newName = new FullName("New", "Name");

        Assert.Throws<DomainException>(() =>
        {
            user.UpdateFullName(newName);
        });
    }
}