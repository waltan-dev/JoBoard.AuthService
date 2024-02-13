using JoBoard.AuthService.Domain.Events;
using MediatR;

namespace JoBoard.AuthService.Application.DomainEventHandlers;

public class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}