using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class PasswordStrengthValidatorStubFactory
{
    public IPasswordStrengthValidator Create()
    {
        return new PasswordStrengthValidator();
    }
}