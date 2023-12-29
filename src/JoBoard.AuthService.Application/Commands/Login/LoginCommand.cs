using MediatR;

namespace JoBoard.AuthService.Application.Accounts;

public class LoginCommand : IRequest<AuthInfo>
{
    public string Email { get; set; }
    public string Password { get; set; }
}