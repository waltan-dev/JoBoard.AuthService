using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserChangedRoleDomainEvent : INotification
{
    public User User { get; }

    public UserChangedRoleDomainEvent(User user)
    {
        User = user;
    }
}