using System.Security.Claims;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public static class ClaimsPrincipalExtensions
{
    public static UserId? GetUserId(this ClaimsPrincipal principal)
    {
        // get user id from JWT
        string? userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdStr, out Guid userIdGuid) 
            ? UserId.FromValue(userIdGuid) 
            : null;
    }
}