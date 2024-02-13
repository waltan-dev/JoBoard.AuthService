using JoBoard.AuthService.Domain.Aggregates.User.Events;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.DomainEventHandlers;

public class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}