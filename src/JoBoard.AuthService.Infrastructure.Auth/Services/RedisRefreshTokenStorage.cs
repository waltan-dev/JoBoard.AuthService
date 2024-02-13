using System.Text.Json;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace JoBoard.AuthService.Infrastructure.Auth.Services;

public class RedisRefreshTokenStorage : IRefreshTokenStorage
{
    private readonly IDistributedCache _distributedCache;

    public RedisRefreshTokenStorage(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task AddTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct)
    {
        var refreshTokens = await GetTokensAsync(userId, ct);
        refreshTokens.Add(refreshToken);
        await UpdateTokensAsync(userId, refreshTokens, ct);
    }

    public async Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct)
    {
        var currentRefreshTokensBytes = await _distributedCache.GetAsync(userId, ct);
        return currentRefreshTokensBytes == null 
            ? new List<RefreshToken>() 
            : JsonSerializer.Deserialize<List<RefreshToken>>(currentRefreshTokensBytes)!;
    }

    public async Task RemoveTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct)
    {
        var refreshTokens = await GetTokensAsync(userId, ct);
        refreshTokens.Remove(refreshToken);
        await UpdateTokensAsync(userId, refreshTokens, ct);
    }
    
    private async Task UpdateTokensAsync(string userId, List<RefreshToken> refreshTokens, CancellationToken ct)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(refreshTokens);
        await _distributedCache.SetAsync(userId, bytes, ct);
    }
}