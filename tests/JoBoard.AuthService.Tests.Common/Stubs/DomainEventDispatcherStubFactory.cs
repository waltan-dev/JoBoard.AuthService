using JoBoard.AuthService.Domain.Common.SeedWork;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public static class DomainEventDispatcherStubFactory
{
    public static IDomainEventDispatcher Create()
    {
        return new Mock<IDomainEventDispatcher>().Object;
    }
}