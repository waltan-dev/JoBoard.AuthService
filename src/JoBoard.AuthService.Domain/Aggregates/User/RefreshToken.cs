using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class RefreshToken : Entity
{
    public new Guid Id { get; }
    public UserId UserId { get; }
    public string Token { get; }
    public DateTime ExpiresAt { get; }
    
    private RefreshToken() {} // for ef core only
    
    public RefreshToken(Guid id, UserId userId, string token, int tokenExpiresInHours)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAt = DateTime.UtcNow.AddHours(tokenExpiresInHours).TrimMilliseconds();
    }
}