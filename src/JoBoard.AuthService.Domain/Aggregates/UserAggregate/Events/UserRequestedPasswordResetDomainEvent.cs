using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserRequestedPasswordResetDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedPasswordResetDomainEvent(User user)
    {
        User = user;
    }
}