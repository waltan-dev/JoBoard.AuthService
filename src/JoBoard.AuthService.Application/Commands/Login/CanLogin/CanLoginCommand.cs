using JoBoard.AuthService.Application.Models;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLogin;

// immutable command
public class CanLoginCommand : IRequest<LoginResult>
{
    public string UserId { get; init; }
}