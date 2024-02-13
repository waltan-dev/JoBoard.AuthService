using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Common.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class PasswordStrengthValidatorStubFactory
{
    public IPasswordStrengthValidator Create()
    {
        return new PasswordStrengthValidator();
    }
}