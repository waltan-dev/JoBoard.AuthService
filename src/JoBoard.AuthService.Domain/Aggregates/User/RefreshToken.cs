using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class RefreshToken : Entity
{
    public new Guid Id { get; private set; }
    public UserId UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    private RefreshToken() {} // for ef core only
    
    private RefreshToken(Guid id, UserId userId, string token, int tokenExpiresInHours)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAt = DateTime.UtcNow.AddHours(tokenExpiresInHours).TrimMilliseconds();
    }

    internal static RefreshToken Create(UserId userId, int tokenExpiresInHours, ISecureTokenizer secureTokenizer)
    {
        var secureToken = secureTokenizer.Generate();
        return new RefreshToken(Guid.NewGuid(), userId, secureToken, tokenExpiresInHours);
    }
}