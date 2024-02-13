using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByEmail;

public class LoginByEmailCommand : IRequest<UserResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}