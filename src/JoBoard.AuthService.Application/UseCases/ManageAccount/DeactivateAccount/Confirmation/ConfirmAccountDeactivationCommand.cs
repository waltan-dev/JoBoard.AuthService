using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.DeactivateAccount.Confirmation;

// immutable command
public class ConfirmAccountDeactivationCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; init; }
}