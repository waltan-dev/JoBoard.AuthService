using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserDetachedExternalAccountDomainEvent : INotification
{
    public User User { get; }

    public UserDetachedExternalAccountDomainEvent(User user)
    {
        User = user;
    }
}