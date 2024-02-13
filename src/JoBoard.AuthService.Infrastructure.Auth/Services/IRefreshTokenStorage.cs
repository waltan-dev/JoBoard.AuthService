using JoBoard.AuthService.Infrastructure.Auth.Models;

namespace JoBoard.AuthService.Infrastructure.Auth.Services;

public interface IRefreshTokenStorage
{
    Task AddTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
    Task<List<RefreshToken>> GetTokensAsync(string userId, CancellationToken ct);
    Task RemoveTokenAsync(string userId, RefreshToken refreshToken, CancellationToken ct);
}