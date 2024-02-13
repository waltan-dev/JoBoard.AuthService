using JoBoard.AuthService.Application.Contracts;
using Moq;

namespace JoBoard.AuthService.Tests.Unit.Stubs;

public class DomainEventDispatcherStubFactory
{
    public IDomainEventDispatcher Create()
    {
        return new Mock<IDomainEventDispatcher>().Object;
    }
}