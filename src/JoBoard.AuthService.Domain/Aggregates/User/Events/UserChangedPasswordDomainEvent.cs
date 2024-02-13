using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserChangedPasswordDomainEvent : INotification
{
    public User User { get; }

    public UserChangedPasswordDomainEvent(User user)
    {
        User = user;
    }
}