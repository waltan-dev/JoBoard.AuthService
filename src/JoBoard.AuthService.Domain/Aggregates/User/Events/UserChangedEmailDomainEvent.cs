using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserChangedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserChangedEmailDomainEvent(User user)
    {
        User = user;
    }
}