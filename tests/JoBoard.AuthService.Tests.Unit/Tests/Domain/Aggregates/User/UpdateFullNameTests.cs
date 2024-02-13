using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class UpdateFullNameTests
{
    [Fact]
    public void UpdateFullName()
    {
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newName = new FullName("New", "Name");

        user.UpdateFullName(newName);
        
        Assert.Equal(newName, user.FullName);
        Assert.Single(user.DomainEvents, ev => ev is UserUpdatedNameDomainEvent);
    }
    
    [Fact]
    public void UpdateFullNameWithInactiveStatus()
    {
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var newName = new FullName("New", "Name");

        Assert.Throws<DomainException>(() =>
        {
            user.UpdateFullName(newName);
        });
    }
}