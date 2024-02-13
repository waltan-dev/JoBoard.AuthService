using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.ChangePassword;

public class ChangePasswordCommand : IRequest<Unit>
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}