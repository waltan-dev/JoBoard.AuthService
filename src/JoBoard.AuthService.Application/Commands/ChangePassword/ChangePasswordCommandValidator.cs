using FluentValidation;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Application.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(IPasswordStrengthValidator passwordStrengthValidator)
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password can't be empty");
        
        RuleFor(c => c.NewPassword)
            .Must(passwordStrengthValidator.Validate)
            .WithMessage($"Password must contain between {passwordStrengthValidator.MinPasswordLength} " +
                         $"and {passwordStrengthValidator.MaxPasswordLength} characters, " +
                         "at least one lowercase letter, " +
                         "one uppercase letter, one digit and one non-alphanumeric character");
    }
}