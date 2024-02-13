using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserRequestedAccountDeactivationDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedAccountDeactivationDomainEvent(User user)
    {
        User = user;
    }
}