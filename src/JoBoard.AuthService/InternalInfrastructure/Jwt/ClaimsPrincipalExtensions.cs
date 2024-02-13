using System.Security.Claims;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        // get user id from JWT
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}