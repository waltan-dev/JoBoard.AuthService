using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login;

public class LoginCommand : IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}