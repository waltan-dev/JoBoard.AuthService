using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class PasswordHasherStubFactory
{
    public IPasswordHasher Create()
    {
        return new PasswordHasher();
    }
}