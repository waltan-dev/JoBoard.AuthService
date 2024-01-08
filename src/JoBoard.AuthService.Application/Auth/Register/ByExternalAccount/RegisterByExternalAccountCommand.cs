using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Register.ByExternalAccount;

public class RegisterByExternalAccountCommand : IRequest<Unit>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
    public string ExternalUserId { get; set; }
}