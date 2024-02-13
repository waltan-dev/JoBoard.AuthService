using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserAttachedExternalAccountDomainEvent : INotification
{
    public User User { get; }

    public UserAttachedExternalAccountDomainEvent(User user)
    {
        User = user;
    }
}