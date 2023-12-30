using MediatR;

namespace JoBoard.AuthService.Application.Commands.RequestPasswordReset;

public class RequestPasswordResetCommand : IRequest
{
    public string Email { get; set; }
}