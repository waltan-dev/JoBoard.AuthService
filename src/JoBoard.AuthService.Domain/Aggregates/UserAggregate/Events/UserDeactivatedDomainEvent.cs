using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserDeactivatedDomainEvent : INotification
{
    public User User { get; }

    public UserDeactivatedDomainEvent(User user)
    {
        User = user;
    }
}