using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.ChangeEmail.Confirmation;

public class ChangeEmailCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}