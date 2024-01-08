using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;

public class ResetPasswordCommand : IRequest<Unit>
{
    public string Email { get; set; }
    public string ConfirmationToken { get; set; }
    public string NewPassword { get; set; }
}