using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Request;

// immutable command
public class RequestPasswordResetCommand : IRequest<Unit>
{
    public string Email { get; init; }
}