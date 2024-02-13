using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserDeactivatedDomainEvent : INotification
{
    public User User { get; }

    public UserDeactivatedDomainEvent(User user)
    {
        User = user;
    }
}