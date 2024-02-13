using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;

public class UserRequestedEmailConfirmationDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedEmailConfirmationDomainEvent(User user)
    {
        User = user;
    }
}