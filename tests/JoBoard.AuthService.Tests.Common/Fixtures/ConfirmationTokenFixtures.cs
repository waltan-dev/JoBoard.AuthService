using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class ConfirmationTokenFixtures
{
    public static ConfirmationToken CreateNew()
    {
        return ConfirmationToken.Create(GetSecureTokenizerStub(), TimeSpan.FromHours(24));
    }
    
    public static ConfirmationToken CreateExpired()
    {
        return ConfirmationToken.Create(GetSecureTokenizerStub(), TimeSpan.FromHours(-1));
    }

    public static ISecureTokenizer GetSecureTokenizerStub()
    {
        return new SecureTokenizer();
    }

    public static ConfirmationTokenConfig GetConfirmationTokenConfig()
    {
        return new ConfirmationTokenConfig { TokenLifeSpan = TimeSpan.FromHours(24) };
    }
}