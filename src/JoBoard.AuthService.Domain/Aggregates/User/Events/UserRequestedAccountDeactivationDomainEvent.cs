using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserRequestedAccountDeactivationDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedAccountDeactivationDomainEvent(User user)
    {
        User = user;
    }
}