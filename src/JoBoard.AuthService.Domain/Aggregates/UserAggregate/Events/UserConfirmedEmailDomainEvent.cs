using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserConfirmedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserConfirmedEmailDomainEvent(User user)
    {
        User = user;
    }
}