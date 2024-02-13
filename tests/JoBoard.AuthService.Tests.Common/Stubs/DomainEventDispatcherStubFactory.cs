using JoBoard.AuthService.Domain.Common.SeedWork;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

// Thread-safe Singleton
internal static class DomainEventDispatcherStubFactory
{
    public static IDomainEventDispatcher Create()
    {
        return new Mock<IDomainEventDispatcher>().Object;
    }
}