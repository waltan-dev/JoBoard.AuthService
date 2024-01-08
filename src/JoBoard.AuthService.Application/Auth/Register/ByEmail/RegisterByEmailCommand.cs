using MediatR;

namespace JoBoard.AuthService.Application.Auth.Register.ByEmail;

public class RegisterByEmailCommand : IRequest<Unit>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}