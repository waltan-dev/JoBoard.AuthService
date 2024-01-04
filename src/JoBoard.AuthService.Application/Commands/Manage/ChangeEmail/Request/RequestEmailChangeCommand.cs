using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.ChangeEmail.Request;

public class RequestEmailChangeCommand : IRequest
{
    public string NewEmail { get; set; }
}