using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Confirmation;

public class ChangeEmailCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}