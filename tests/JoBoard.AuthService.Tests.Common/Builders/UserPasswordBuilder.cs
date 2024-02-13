using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserPasswordBuilder
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;

    public UserPasswordBuilder(IPasswordHasher passwordHasher, IPasswordStrengthValidator passwordStrengthValidator)
    {
        _passwordHasher = passwordHasher;
        _passwordStrengthValidator = passwordStrengthValidator;
    }
    
    public UserPassword Create(string password)
    {
        return UserPassword.Create(password, _passwordStrengthValidator, _passwordHasher);
    }
}