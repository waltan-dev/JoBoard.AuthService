using MediatR;

namespace JoBoard.AuthService.Application.Commands.ManageExternalAccount.AttachGoogleAccount;

// immutable command
public class AttachGoogleAccountCommand : IRequest<Unit>
{
    public string GoogleIdToken { get; init; }
}