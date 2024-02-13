using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserBlockedDomainEvent : INotification
{
    public User User { get; }

    public UserBlockedDomainEvent(User user)
    {
        User = user;
    }
}