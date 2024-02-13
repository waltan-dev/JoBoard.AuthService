using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserRegisteredDomainEvent : INotification
{
    public User User { get; }

    public UserRegisteredDomainEvent(User user)
    {
        User = user;
    }
}