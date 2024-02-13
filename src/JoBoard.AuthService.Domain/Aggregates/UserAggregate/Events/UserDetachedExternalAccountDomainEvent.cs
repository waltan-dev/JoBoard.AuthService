using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserDetachedExternalAccountDomainEvent : INotification
{
    public User User { get; }

    public UserDetachedExternalAccountDomainEvent(User user)
    {
        User = user;
    }
}