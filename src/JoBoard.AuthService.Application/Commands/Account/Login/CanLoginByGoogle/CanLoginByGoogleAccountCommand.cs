using JoBoard.AuthService.Application.Models;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Account.Login.CanLoginByGoogle;

// immutable command
public class CanLoginByGoogleAccountCommand : IRequest<LoginResult>
{
    public string GoogleIdToken { get; init; }
}