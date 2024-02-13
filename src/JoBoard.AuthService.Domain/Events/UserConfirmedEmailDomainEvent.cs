using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserConfirmedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserConfirmedEmailDomainEvent(User user)
    {
        User = user;
    }
}