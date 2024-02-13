using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.CanLoginByPassword;

public class CanLoginByPasswordCommand : IRequest<LoginResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}