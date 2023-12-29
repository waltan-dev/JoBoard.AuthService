using MediatR;

namespace JoBoard.AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest
{
    public string Token { get; set; }
}