using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangePassword;

// immutable command
public class ChangePasswordCommand : IRequest<Unit>
{
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
}