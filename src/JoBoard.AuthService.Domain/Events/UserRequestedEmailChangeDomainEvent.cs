using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserRequestedEmailChangeDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedEmailChangeDomainEvent(User user)
    {
        User = user;
    }
}