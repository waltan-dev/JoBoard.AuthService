using MediatR;

namespace JoBoard.AuthService.Application.Commands.Account.Register.ByEmailAndPassword;

// immutable command
public class RegisterByEmailAndPasswordCommand : IRequest<Unit>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
}