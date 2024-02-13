using MediatR;

namespace JoBoard.AuthService.Domain.Aggregates.User.Events;

public class UserRequestedEmailConfirmationDomainEvent : INotification
{
    public User User { get; }

    public UserRequestedEmailConfirmationDomainEvent(User user)
    {
        User = user;
    }
}