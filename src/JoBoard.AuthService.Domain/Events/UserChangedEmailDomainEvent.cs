using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserChangedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserChangedEmailDomainEvent(User user)
    {
        User = user;
    }
}