using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;

namespace JoBoard.AuthService.Application.Commands.Register.ByGoogle;

public class RegisterByGoogleAccountCommandValidator : AbstractValidator<RegisterByGoogleAccountCommand>
{
    public RegisterByGoogleAccountCommandValidator()
    {
        RuleFor(c => c.Role)
            .Must(value => 
                UserRole.Hirer.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase) 
                || UserRole.Worker.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Role must be Hirer or Worker");
    }
}