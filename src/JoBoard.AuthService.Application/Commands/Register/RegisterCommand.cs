using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Register;

public class RegisterCommand : IRequest<AuthInfo>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public AccountType AccountType { get; set; }
}