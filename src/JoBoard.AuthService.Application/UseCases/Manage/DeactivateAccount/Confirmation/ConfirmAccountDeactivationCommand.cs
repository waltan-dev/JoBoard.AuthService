using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.DeactivateAccount.Confirmation;

public class ConfirmAccountDeactivationCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}