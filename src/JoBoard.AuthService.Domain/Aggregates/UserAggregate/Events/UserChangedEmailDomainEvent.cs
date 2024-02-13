using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserChangedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserChangedEmailDomainEvent(User user)
    {
        User = user;
    }
}