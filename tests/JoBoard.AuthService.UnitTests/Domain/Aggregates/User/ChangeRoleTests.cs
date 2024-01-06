﻿using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ChangeRoleTests
{
    [Fact]
    public void ChangeRoleToHirer()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Hirer);
        
        Assert.Equal(UserRole.Hirer, user.Role);
    }
    
    [Fact]
    public void ChangeRoleToWorker()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Worker);
        
        Assert.Equal(UserRole.Worker, user.Role);
    }
    
    [Fact]
    public void ChangeRoleToAdmin()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Admin);
        });
    }
    
    [Fact]
    public void ChangeRoleWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Worker);
        });
    }
}