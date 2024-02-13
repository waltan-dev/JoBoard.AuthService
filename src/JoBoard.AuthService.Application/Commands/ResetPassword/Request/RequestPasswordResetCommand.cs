using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Request;

// immutable command
public class RequestPasswordResetCommand : IRequest<Unit>
{
    public string Email { get; init; }
}