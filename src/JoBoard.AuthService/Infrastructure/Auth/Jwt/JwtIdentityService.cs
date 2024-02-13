using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Infrastructure.Auth.Jwt;

public class JwtIdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserId GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (userId == null)
            throw new NotFoundException("User not found");
        return userId;
    }
}