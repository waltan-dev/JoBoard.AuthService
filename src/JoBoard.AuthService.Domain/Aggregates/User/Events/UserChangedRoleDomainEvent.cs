using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserChangedRoleDomainEvent : INotification
{
    public User User { get; }

    public UserChangedRoleDomainEvent(User user)
    {
        User = user;
    }
}