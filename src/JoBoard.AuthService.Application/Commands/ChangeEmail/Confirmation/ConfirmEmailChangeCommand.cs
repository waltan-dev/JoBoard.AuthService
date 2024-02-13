using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeEmail.Confirmation;

// immutable command
public class ConfirmEmailChangeCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; init; }
}