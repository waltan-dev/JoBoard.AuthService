using MediatR;

namespace JoBoard.AuthService.Application.Commands.Account.ConfirmEmail;

// immutable command
public class ConfirmEmailCommand : IRequest<Unit>
{
    public Guid UserId { get; init; }
    public string Token { get; init; }
}