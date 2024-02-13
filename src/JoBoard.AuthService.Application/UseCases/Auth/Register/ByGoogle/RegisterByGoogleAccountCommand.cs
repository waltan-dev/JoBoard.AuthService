using JoBoard.AuthService.Application.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;

public class RegisterByGoogleAccountCommand : IRequest<UserResult>
{
    public string Role { get; set; }
    public string GoogleIdToken { get; set; }
}