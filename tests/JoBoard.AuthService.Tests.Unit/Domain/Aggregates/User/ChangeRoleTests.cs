using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.Events;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class ChangeRoleTests
{
    [Fact]
    public void ChangeRoleToHirer()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Hirer);
        
        Assert.Equal(UserRole.Hirer, user.Role);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedRoleDomainEvent);
    }
    
    [Fact]
    public void ChangeRoleToWorker()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Worker);
        
        Assert.Equal(UserRole.Worker, user.Role);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedRoleDomainEvent);
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
    public void ChangeRoleWithoutConfirmedEmail()
    {
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Worker);
        });
    }
    
    [Fact]
    public void ChangeRoleWithInactiveStatus()
    {
        var user = new UserBuilder().WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Worker);
        });
    }
}