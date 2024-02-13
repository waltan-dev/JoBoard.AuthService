using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserRequestedEmailChangeDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedEmailChangeDomainEvent(User user)
    {
        User = user;
    }
}