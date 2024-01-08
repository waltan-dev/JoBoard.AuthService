using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Application.UseCases.Auth.Manage.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password can't be empty");
        
        RuleFor(c => c.NewPassword)
            .Must(PasswordStrengthChecker.Check)
            .WithMessage($"Password must contain between {PasswordStrengthChecker.MinPasswordLength} " +
                         $"and {PasswordStrengthChecker.MaxPasswordLength} characters, " +
                         "at least one lowercase letter, " +
                         "one uppercase letter, one digit and one non-alphanumeric character");
    }
}