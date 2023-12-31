using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Request;

public class RequestPasswordResetCommand : IRequest
{
    public string Email { get; set; }
}