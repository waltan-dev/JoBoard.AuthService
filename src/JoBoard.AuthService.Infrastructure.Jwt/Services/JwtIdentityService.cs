using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Infrastructure.Jwt.Extensions;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.Infrastructure.Jwt.Services;

public class JwtIdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        // get user id from JWT
        return _httpContextAccessor.HttpContext!.User.GetUserId();
    }
}