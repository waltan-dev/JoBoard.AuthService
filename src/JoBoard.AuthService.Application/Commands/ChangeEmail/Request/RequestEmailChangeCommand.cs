using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeEmail.Request;

public class RequestEmailChangeCommand : IRequest
{
    public string NewEmail { get; set; }
}