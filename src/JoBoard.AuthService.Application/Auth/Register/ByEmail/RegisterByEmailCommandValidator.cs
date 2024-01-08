using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application.Auth.Register.ByEmail;

public class RegisterByEmailCommandValidator : AbstractValidator<RegisterByEmailCommand>
{
    public RegisterByEmailCommandValidator()
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
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must contain at least 6 characters");
        
        RuleFor(c => c.Role)
            .Must(value => 
                UserRole.Hirer.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase) 
                || UserRole.Worker.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Role must be Hirer or Worker");
    }
}