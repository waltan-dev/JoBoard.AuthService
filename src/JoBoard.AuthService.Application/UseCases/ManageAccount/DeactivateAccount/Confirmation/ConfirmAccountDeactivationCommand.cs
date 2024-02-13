using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.DeactivateAccount.Confirmation;

public class ConfirmAccountDeactivationCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}