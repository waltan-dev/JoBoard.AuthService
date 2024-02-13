using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserChangedRoleDomainEvent : INotification
{
    public User User { get; }

    public UserChangedRoleDomainEvent(User user)
    {
        User = user;
    }
}