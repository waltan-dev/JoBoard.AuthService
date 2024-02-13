using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Domain.Events;

public class UserRequestedPasswordResetDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedPasswordResetDomainEvent(User user)
    {
        User = user;
    }
}