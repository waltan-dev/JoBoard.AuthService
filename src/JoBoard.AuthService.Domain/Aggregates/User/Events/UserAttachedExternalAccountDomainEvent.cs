using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserAttachedExternalAccountDomainEvent : INotification
{
    public User User { get; }

    public UserAttachedExternalAccountDomainEvent(User user)
    {
        User = user;
    }
}