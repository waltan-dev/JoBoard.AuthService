using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.AttachExternalAccount;

public class AttachExternalAccountCommand : IRequest<Unit>
{
    public string ExternalUserId { get; set; }
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
}