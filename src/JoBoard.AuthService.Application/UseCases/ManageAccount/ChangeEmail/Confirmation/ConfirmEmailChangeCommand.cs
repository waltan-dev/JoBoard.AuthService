using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangeEmail.Confirmation;

public class ConfirmEmailChangeCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}