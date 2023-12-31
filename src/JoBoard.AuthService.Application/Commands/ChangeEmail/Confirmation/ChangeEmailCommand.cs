using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeEmail.Confirmation;

public class ChangeEmailCommand : IRequest
{
    public string ConfirmationToken { get; set; }
}