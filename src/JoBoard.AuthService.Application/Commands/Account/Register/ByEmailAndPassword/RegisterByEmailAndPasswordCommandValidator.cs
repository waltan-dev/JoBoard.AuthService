using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Application.Commands.Account.Register.ByEmailAndPassword;

public class RegisterByEmailAndPasswordCommandValidator : AbstractValidator<RegisterByEmailAndPasswordCommand>
{
    public RegisterByEmailAndPasswordCommandValidator(IPasswordStrengthValidator passwordStrengthValidator)
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithMessage("First name can't be empty");
        
        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithMessage("Last name can't be empty");
        
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Email must be valid email address");
        
        RuleFor(c => c.Password)
            .Must(passwordStrengthValidator.Validate)
            .WithMessage($"Password must contain between {passwordStrengthValidator.MinPasswordLength} " +
                         $"and {passwordStrengthValidator.MaxPasswordLength} characters, " +
                         "at least one lowercase letter, " +
                         "one uppercase letter, one digit and one non-alphanumeric character");
        
        RuleFor(c => c.Role)
            .Must(value => 
                UserRole.Hirer.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase) 
                || UserRole.Worker.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Role must be Hirer or Worker");
    }
}