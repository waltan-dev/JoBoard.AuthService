using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class PasswordBuilder
{
    private readonly IPasswordHasher _passwordHasher = PasswordHasherStubFactory.Create();
    private readonly IPasswordStrengthValidator _passwordValidator = PasswordStrengthValidatorStubFactory.Create();
    
    public PasswordHash Create(string password)
    {
        return PasswordHash.Create(password, _passwordValidator, _passwordHasher);
    }
}