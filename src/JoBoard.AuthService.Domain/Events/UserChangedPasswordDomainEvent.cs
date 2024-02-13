using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserChangedPasswordDomainEvent : INotification
{
    public User User { get; }

    public UserChangedPasswordDomainEvent(User user)
    {
        User = user;
    }
}