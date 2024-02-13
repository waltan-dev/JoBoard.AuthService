using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Confirmation;

public class ConfirmEmailChangeCommand : IRequest<Unit>
{
    public string ConfirmationToken { get; set; }
}