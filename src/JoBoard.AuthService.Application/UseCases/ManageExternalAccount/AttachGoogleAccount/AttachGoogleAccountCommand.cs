using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageExternalAccount.AttachGoogleAccount;

public class AttachGoogleAccountCommand : IRequest<Unit>
{
    public string GoogleIdToken { get; set; }
}