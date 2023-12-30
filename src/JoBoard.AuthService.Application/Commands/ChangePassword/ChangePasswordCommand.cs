using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}