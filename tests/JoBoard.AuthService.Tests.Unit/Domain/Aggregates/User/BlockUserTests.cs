using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Tests.Common.Builders;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class BlockUserTests
{
    [Fact]
    public void Block()
    {
        var user = TestsRegistry.UserBuilder.Build();
        
        user.Block();
        
        Assert.Equal(UserStatus.Blocked, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserBlockedDomainEvent);
    }
}