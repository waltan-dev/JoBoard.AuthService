using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class ChangeRoleTests
{
    [Fact]
    public void ChangeRoleToHirer()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Hirer);
        
        Assert.Equal(UserRole.Hirer, user.Role);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedRoleDomainEvent);
    }
    
    [Fact]
    public void ChangeRoleToWorker()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        
        user.ChangeRole(UserRole.Worker);
        
        Assert.Equal(UserRole.Worker, user.Role);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedRoleDomainEvent);
    }
    
    [Fact]
    public void ChangeRoleToAdmin()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Admin);
        });
    }
    
    [Fact]
    public void ChangeRoleWithoutConfirmedEmail()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Worker);
        });
    }
    
    [Fact]
    public void ChangeRoleWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.ChangeRole(UserRole.Worker);
        });
    }
}