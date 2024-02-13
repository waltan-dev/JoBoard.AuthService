using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserChangedPasswordDomainEvent : INotification
{
    public User User { get; }

    public UserChangedPasswordDomainEvent(User user)
    {
        User = user;
    }
}