﻿using System.Security.Claims;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public UserId GetUserId()
    {
        string? userIdStr = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _ = Guid.TryParse(userIdStr, out Guid userIdGuid);
        return new UserId(userIdGuid);
    }
}