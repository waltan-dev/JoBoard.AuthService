using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class PasswordBuilder
{
    public PasswordHash Create(string password)
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var passwordValidator = PasswordFixtures.GetPasswordStrengthValidatorStub();
        return PasswordHash.Create(password, passwordValidator, passwordHasher);
    }
}