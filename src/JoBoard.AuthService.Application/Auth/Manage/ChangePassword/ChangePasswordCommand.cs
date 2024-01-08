using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.ChangePassword;

public class ChangePasswordCommand : IRequest<Unit>
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}