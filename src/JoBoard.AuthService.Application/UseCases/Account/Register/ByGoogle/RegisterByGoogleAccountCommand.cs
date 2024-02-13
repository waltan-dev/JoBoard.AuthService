using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Register.ByGoogle;

public class RegisterByGoogleAccountCommand : IRequest<Unit>
{
    public string Role { get; set; }
    public string GoogleIdToken { get; set; }
}