using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangeEmail.Request;

public class RequestEmailChangeCommand : IRequest<Unit>
{
    public string NewEmail { get; set; }
}