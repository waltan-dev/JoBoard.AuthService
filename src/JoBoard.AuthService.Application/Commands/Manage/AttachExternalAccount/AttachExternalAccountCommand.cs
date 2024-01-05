using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.AttachExternalAccount;

public class AttachExternalAccountCommand : IRequest
{
    public string ExternalUserId { get; set; }
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
}