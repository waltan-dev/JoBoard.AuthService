using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Request;

public class RequestEmailChangeCommand : IRequest<Unit>
{
    public string NewEmail { get; set; }
}