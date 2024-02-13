using System.Text.Json;
using JoBoard.AuthService.Infrastructure.Auth.Models;
using StackExchange.Redis;

namespace JoBoard.AuthService.Infrastructure.Auth.Services;

public interface IRefreshTokenRepository
{
    Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct);
    Task<bool> AddTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
    Task<bool> RemoveTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
}

public class RedisRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabase _database;
    private static string KeyPrefix = "/auth/refresh-tokens/";
    private static RedisKey GetKey(string userId) => KeyPrefix + userId;
    
    public RedisRefreshTokenRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct)
    {
        using var currentRefreshTokensBytes = await _database.StringGetLeaseAsync(GetKey(userId));
        return currentRefreshTokensBytes is null || currentRefreshTokensBytes.Length == 0
            ? new List<RefreshToken>() 
            : JsonSerializer.Deserialize<List<RefreshToken>>(currentRefreshTokensBytes.Span)!;
    }
    
    public async Task<bool> AddTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct)
    {
        var refreshTokens = await GetTokensAsync(userId, ct);
        refreshTokens.Add(refreshToken);
        return await UpdateTokensAsync(userId, refreshTokens, ct);
    }
    
    public async Task<bool> RemoveTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct)
    {
        var refreshTokens = await GetTokensAsync(userId, ct);
        refreshTokens.Remove(refreshToken);
        return await UpdateTokensAsync(userId, refreshTokens, ct);
    }
    
    private async Task<bool> UpdateTokensAsync(string userId, List<RefreshToken> refreshTokens, CancellationToken ct)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(refreshTokens);
        return await _database.StringSetAsync(GetKey(userId), bytes);
    }
}