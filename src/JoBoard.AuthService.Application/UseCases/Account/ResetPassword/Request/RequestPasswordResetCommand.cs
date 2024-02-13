using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Request;

public class RequestPasswordResetCommand : IRequest<Unit>
{
    public string Email { get; set; }
}