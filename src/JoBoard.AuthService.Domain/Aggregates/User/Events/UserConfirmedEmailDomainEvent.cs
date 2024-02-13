using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserConfirmedEmailDomainEvent : INotification
{
    public User User { get; }

    public UserConfirmedEmailDomainEvent(User user)
    {
        User = user;
    }
}