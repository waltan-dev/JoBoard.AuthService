using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using MediatR;

namespace JoBoard.AuthService.Application.DomainEventHandlers;

public class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}