using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.Events;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User;

public class BlockUserTests
{
    [Fact]
    public void Block()
    {
        var user = new UserBuilder().Build();
        
        user.Block();
        
        Assert.Equal(UserStatus.Blocked, user.Status);
        Assert.Single(user.DomainEvents, ev => ev is UserBlockedDomainEvent);
    }
}