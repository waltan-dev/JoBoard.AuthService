using FluentValidation;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Request;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Email must be valid email address");
    }
}