using System.Security.Claims;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Infrastructure.Auth.Jwt;

public static class ClaimsPrincipalExtensions
{
    public static UserId? GetUserId(this ClaimsPrincipal principal)
    {
        // parse user id from JWT
        string? userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdStr, out Guid userIdGuid) 
            ? UserId.FromValue(userIdGuid) 
            : null;
    }
}