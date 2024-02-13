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

    public static RefreshToken Create(TimeSpan lifeSpan, ISecureTokenizer secureTokenizer)
    {
        var secureToken = secureTokenizer.Generate();
        return new RefreshToken(secureToken, DateTime.UtcNow.Add(lifeSpan));
    }
}