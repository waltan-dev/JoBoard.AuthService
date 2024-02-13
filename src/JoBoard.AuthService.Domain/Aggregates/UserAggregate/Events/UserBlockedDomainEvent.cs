using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserBlockedDomainEvent : INotification
{
    public User User { get; }

    public UserBlockedDomainEvent(User user)
    {
        User = user;
    }
}