using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.CanLoginByGoogle;

public class CanLoginByGoogleAccountCommand : IRequest<LoginResult>
{
    public string GoogleIdToken { get; set; }
}