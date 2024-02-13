using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserUpdatedNameDomainEvent : INotification
{
    public User User { get; }

    public UserUpdatedNameDomainEvent(User user)
    {
        User = user;
    }
}