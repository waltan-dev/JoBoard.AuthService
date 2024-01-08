using MediatR;

namespace JoBoard.AuthService.Application.Auth.ResetPassword.Request;

public class RequestPasswordResetCommand : IRequest<Unit>
{
    public string Email { get; set; }
}