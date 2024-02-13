using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;

public class RegisterByGoogleAccountCommand : IRequest<Unit>
{
    public string Role { get; set; }
    public string GoogleIdToken { get; set; }
}