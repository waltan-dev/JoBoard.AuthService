﻿using FluentValidation;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;

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
            .Must(PasswordStrengthChecker.Check)
            .WithMessage($"Password must contain between {PasswordStrengthChecker.MinPasswordLength} " +
                         $"and {PasswordStrengthChecker.MaxPasswordLength} characters, " +
                         "at least one lowercase letter, " +
                         "one uppercase letter, one digit and one non-alphanumeric character");
        
        RuleFor(c => c.Role)
            .Must(value => 
                UserRole.Hirer.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase) 
                || UserRole.Worker.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Role must be Hirer or Worker");
    }
}