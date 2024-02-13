using JoBoard.AuthService.Application.Services;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public class JwtIdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        // get user id from JWT
        return _httpContextAccessor.HttpContext!.User.GetUserId();
    }
}