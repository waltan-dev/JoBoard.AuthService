using System.Text.Json;
using JoBoard.AuthService.Infrastructure.Auth.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace JoBoard.AuthService.Infrastructure.Auth.Services;

public interface IRefreshTokenStorage
{
    Task AddTokenAsync(string userId, RefreshToken refreshToken);
    Task<List<RefreshToken>> GetTokensAsync(string userId);
    Task RemoveTokenAsync(string userId, RefreshToken refreshToken);
}

public class RedisRefreshTokenStorage : IRefreshTokenStorage
{
    private readonly IDistributedCache _distributedCache;

    public RedisRefreshTokenStorage(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task AddTokenAsync(string userId, RefreshToken refreshToken)
    {
        var refreshTokens = await GetTokensAsync(userId);
        refreshTokens.Add(refreshToken);
        await UpdateRefreshTokensAsync(userId, refreshTokens);
    }

    public async Task<List<RefreshToken>> GetTokensAsync(string userId)
    {
        var currentRefreshTokensBytes = await _distributedCache.GetAsync(userId);
        return currentRefreshTokensBytes == null 
            ? new List<RefreshToken>() 
            : JsonSerializer.Deserialize<List<RefreshToken>>(currentRefreshTokensBytes)!;
    }

    public async Task RemoveTokenAsync(string userId, RefreshToken refreshToken)
    {
        var refreshTokens = await GetTokensAsync(userId);
        refreshTokens.Remove(refreshToken);
        await UpdateRefreshTokensAsync(userId, refreshTokens);
    }
    
    private async Task UpdateRefreshTokensAsync(string userId, List<RefreshToken> refreshTokens)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(refreshTokens);
        await _distributedCache.SetAsync(userId, bytes);
    }
}