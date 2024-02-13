using JoBoard.AuthService.Domain.Common.SeedWork;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class EventFixtures
{
    public static IDomainEventDispatcher GetDomainEventDispatcherStub()
    {
        return new Mock<IDomainEventDispatcher>().Object;
    }
}