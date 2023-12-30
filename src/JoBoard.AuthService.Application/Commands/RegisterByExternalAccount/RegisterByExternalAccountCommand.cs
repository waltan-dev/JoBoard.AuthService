using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.RegisterByExternalAccount;

public class RegisterByExternalAccountCommand : IRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccountType AccountType { get; set; }
    
    public ExternalNetwork ExternalNetwork { get; set; }
    public string ExternalUserId { get; set; }
}