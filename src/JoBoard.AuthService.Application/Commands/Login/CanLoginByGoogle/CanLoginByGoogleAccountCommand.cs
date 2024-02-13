using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLoginByGoogle;

// immutable command
public class CanLoginByGoogleAccountCommand : IRequest<LoginResult>
{
    public string GoogleIdToken { get; init; }
}