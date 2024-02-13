using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLoginByPassword;

// immutable command
public class CanLoginByPasswordCommand : IRequest<LoginResult>
{
    public string Email { get; init; }
    public string Password { get; init; }
}