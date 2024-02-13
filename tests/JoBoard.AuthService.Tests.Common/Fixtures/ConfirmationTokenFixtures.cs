using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Authentication;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class ConfirmationTokenFixtures
{
    public static ConfirmationToken CreateNew()
    {
        return ConfirmationToken.Create(GetSecureTokenizerStub(), 24);
    }
    
    public static ConfirmationToken CreateExpired()
    {
        return ConfirmationToken.Create(GetSecureTokenizerStub(), -1);
    }

    public static ISecureTokenizer GetSecureTokenizerStub()
    {
        return new SecureTokenizer();
    }
}