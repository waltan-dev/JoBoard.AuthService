using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Infrastructure.Auth.Models;

public class RefreshToken
{
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    public RefreshToken(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(int tokenExpiresInHours, ISecureTokenizer secureTokenizer)
    {
        var secureToken = secureTokenizer.Generate();
        return new RefreshToken(secureToken, DateTime.UtcNow.AddHours(tokenExpiresInHours));
    }
}