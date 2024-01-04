using MediatR;

namespace JoBoard.AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}