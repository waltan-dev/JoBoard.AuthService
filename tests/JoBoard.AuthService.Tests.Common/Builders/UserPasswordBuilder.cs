using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Tests.Common.Fixtures;

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
    
    public UserPassword CreateDefault()
    {
        return UserPassword.Create(PasswordFixtures.DefaultPassword, _passwordStrengthValidator, _passwordHasher);
    }
    
    public UserPassword CreateNew()
    {
        return UserPassword.Create(PasswordFixtures.NewPassword, _passwordStrengthValidator, _passwordHasher);
    }
}