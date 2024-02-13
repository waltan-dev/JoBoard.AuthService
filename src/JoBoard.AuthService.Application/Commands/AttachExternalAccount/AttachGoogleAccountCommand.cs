using MediatR;

namespace JoBoard.AuthService.Application.Commands.AttachExternalAccount;

// immutable command
public class AttachGoogleAccountCommand : IRequest<Unit>
{
    public string GoogleIdToken { get; init; }
}