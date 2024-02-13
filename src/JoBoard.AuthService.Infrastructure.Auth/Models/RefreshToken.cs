using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Infrastructure.Auth.Models;

public class RefreshToken
{
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    private RefreshToken(string token, int tokenExpiresInHours)
    {
        Token = token;
        ExpiresAt = DateTime.UtcNow.AddHours(tokenExpiresInHours);
    }

    public static RefreshToken Create(int tokenExpiresInHours, ISecureTokenizer secureTokenizer)
    {
        var secureToken = secureTokenizer.Generate();
        return new RefreshToken(secureToken, tokenExpiresInHours);
    }
}