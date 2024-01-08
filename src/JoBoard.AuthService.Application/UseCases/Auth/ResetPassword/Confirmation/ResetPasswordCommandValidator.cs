using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Email must be valid email address");

        RuleFor(c => c.ConfirmationToken)
            .NotEmpty()
            .WithMessage("Token can't be empty");
        
        RuleFor(c => c.NewPassword)
            .Must(PasswordStrengthChecker.Check)
            .WithMessage($"Password must contain between {PasswordStrengthChecker.MinPasswordLength} " +
                         $"and {PasswordStrengthChecker.MaxPasswordLength} characters, " +
                         "at least one lowercase letter, " +
                         "one uppercase letter, one digit and one non-alphanumeric character");
    }
}