using System.Text.Json;
using JoBoard.AuthService.Infrastructure.Jwt.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace JoBoard.AuthService.Infrastructure.Jwt.Services;

public interface IRefreshTokenRepository
{
    Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct);
    Task<bool> AddTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
    Task<bool> RemoveTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
}

public class RedisRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDistributedCache _cache;

    private const string KeyPrefix = "/auth/refresh-tokens/";
    private static string GetKey(string userId) => KeyPrefix + userId;
    
    public RedisRefreshTokenRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct)
    {
        //using var currentRefreshTokensBytes = await _database.StringGetLeaseAsync(GetKey(userId));
        var currentRefreshTokensBytes = await _cache.GetAsync(GetKey(userId), ct);
        return currentRefreshTokensBytes is null || currentRefreshTokensBytes.Length == 0
            ? new List<RefreshToken>() 
            : JsonSerializer.Deserialize<List<RefreshToken>>(currentRefreshTokensBytes)!;
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
        await _cache.SetAsync(GetKey(userId), bytes, ct);
        return true;
        //return await _database.StringSetAsync(GetKey(userId), bytes);
    }
}