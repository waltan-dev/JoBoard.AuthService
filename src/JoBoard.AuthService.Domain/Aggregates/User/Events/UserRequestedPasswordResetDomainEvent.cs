using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserRequestedPasswordResetDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedPasswordResetDomainEvent(User user)
    {
        User = user;
    }
}