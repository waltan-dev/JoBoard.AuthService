using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}