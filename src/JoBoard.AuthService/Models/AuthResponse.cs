using System.Security.Claims;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Infrastructure.Jwt;

namespace JoBoard.AuthService.Models;

public class AuthResponse : UserResult
{
    public string AccessToken { get; init; }
    
    public static AuthResponse Create(UserResult userResult, IJwtGenerator jwtGenerator)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userResult.UserId.ToString()),
            new(ClaimTypes.Email, userResult.Email),
            new(ClaimTypes.GivenName, userResult.FirstName),
            new(ClaimTypes.Surname, userResult.LastName),
            new(ClaimTypes.Role, userResult.Role)
        };
        var accessToken = jwtGenerator.Generate(claims);

        return new AuthResponse
        {
            UserId = userResult.UserId,
            FirstName = userResult.FirstName,
            LastName = userResult.LastName,
            Email = userResult.Email,
            Role = userResult.Role,
            RefreshToken = userResult.RefreshToken,
            AccessToken = accessToken
        };
    }
}