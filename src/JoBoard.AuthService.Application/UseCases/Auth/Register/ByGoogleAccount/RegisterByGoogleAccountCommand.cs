using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogleAccount;

public class RegisterByGoogleAccountCommand : IRequest<Unit>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    
    public string GoogleIdToken { get; set; }
}