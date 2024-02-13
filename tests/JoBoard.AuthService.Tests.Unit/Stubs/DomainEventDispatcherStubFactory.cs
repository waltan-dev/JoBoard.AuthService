using JoBoard.AuthService.Domain.Common.SeedWork;
using Moq;

namespace JoBoard.AuthService.Tests.Unit.Stubs;

public class DomainEventDispatcherStubFactory
{
    public IDomainEventDispatcher Create()
    {
        return new Mock<IDomainEventDispatcher>().Object;
    }
}