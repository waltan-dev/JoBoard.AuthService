using JoBoard.AuthService.Application.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;

public class LoginByGoogleAccountCommand : IRequest<UserResult>
{
    public string GoogleIdToken { get; set; }
}