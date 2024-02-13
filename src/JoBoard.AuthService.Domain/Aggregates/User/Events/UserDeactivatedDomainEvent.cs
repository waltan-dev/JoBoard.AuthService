using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserDeactivatedDomainEvent : INotification
{
    public User User { get; }

    public UserDeactivatedDomainEvent(User user)
    {
        User = user;
    }
}