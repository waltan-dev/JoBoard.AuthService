using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class PasswordBuilder
{
    private readonly IPasswordHasher _passwordHasher = PasswordFixtures.GetPasswordHasherStub();
    private readonly IPasswordStrengthValidator _passwordStrengthValidator =
        PasswordFixtures.GetPasswordStrengthValidatorStub();
    
    public PasswordHash Create(string password)
    {
        return PasswordHash.Create(password, _passwordStrengthValidator, _passwordHasher);
    }
}