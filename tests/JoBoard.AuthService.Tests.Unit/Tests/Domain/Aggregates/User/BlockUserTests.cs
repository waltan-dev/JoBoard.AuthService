using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class BlockUserTests
{
    [Fact]
    public void Block()
    {
        var user = UnitTestsRegistry.UserBuilder.Build();
        
        user.Block();
        
        Assert.Equal(UserStatus.Blocked, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserBlockedDomainEvent);
    }
}