using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class SecureTokenizerStubFactory
{
    public ISecureTokenizer Create()
    {
        return new SecureTokenizer();
    }
}