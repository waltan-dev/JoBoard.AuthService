using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.AttachExternalAccount;

public class AttachExternalAccountCommand : IRequest
{
    public Guid UserId { get; set; }
    public string ExternalUserId { get; set; }
    public ExternalNetwork ExternalNetwork { get; set; }
}