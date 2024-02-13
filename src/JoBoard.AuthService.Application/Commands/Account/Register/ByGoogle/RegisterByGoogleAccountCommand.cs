using MediatR;

namespace JoBoard.AuthService.Application.Commands.Account.Register.ByGoogle;

// immutable command
public class RegisterByGoogleAccountCommand : IRequest<Unit>
{
    public string Role { get; init; }
    public string GoogleIdToken { get; init; }
}