using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageExternalAccount.AttachGoogleAccount;

// immutable command
public class AttachGoogleAccountCommand : IRequest<Unit>
{
    public string GoogleIdToken { get; init; }
}