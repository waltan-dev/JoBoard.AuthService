using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login;

public class LoginCommand : IRequest<Unit>
{
    public string Email { get; set; }
    public string Password { get; set; }
}