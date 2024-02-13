using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Common.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class PasswordHasherStubFactory
{
    public IPasswordHasher Create()
    {
        return new PasswordHasher();
    }
}