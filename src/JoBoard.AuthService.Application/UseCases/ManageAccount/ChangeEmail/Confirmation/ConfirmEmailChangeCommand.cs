using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangeEmail.Confirmation;

// immutable command
public class ConfirmEmailChangeCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; init; }
}