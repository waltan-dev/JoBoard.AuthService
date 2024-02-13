using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public class JwtIdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserId GetUserId()
    {
        // get user id from JWT
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (userId == null)
            throw new NotFoundException("User not found");
        return userId;
    }
}