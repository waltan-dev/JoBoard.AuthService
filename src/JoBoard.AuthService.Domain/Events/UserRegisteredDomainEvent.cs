using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserRegisteredDomainEvent : INotification
{
    public User User { get; }

    public UserRegisteredDomainEvent(User user)
    {
        User = user;
    }
}