using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogleAccount;

public class RegisterByGoogleAccountCommand : IRequest<Unit>
{
    public string Role { get; set; }
    public string GoogleIdToken { get; set; }
}