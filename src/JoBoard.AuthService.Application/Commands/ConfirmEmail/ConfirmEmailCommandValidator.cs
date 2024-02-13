using FluentValidation;

namespace JoBoard.AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("User ID can't be empty");
        
        RuleFor(c => c.Token)
            .NotEmpty()
            .WithMessage("Token can't be empty");
    }
}