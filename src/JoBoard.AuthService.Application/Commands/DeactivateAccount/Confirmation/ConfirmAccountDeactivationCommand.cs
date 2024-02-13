using MediatR;

namespace JoBoard.AuthService.Application.Commands.DeactivateAccount.Confirmation;

// immutable command
public class ConfirmAccountDeactivationCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; init; }
}