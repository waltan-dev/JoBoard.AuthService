using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserPasswordBuilder
{
    private readonly IPasswordHasher _passwordHasher = PasswordHasherStubFactory.Create();
    private readonly IPasswordStrengthValidator _passwordStrengthValidator =
        PasswordStrengthValidatorStubFactory.Create();
    
    public UserPassword Create(string password)
    {
        return UserPassword.Create(password, _passwordStrengthValidator, _passwordHasher);
    }
}