using System.Security.Claims;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Infrastructure.Auth.Jwt;
using JoBoard.AuthService.Infrastructure.Jwt;

namespace JoBoard.AuthService.Models;

public class UserResponse : UserResult
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    
    public static UserResponse Create(UserResult userResult, IJwtManager jwtManager)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userResult.UserId.ToString()),
            new(ClaimTypes.Email, userResult.Email),
            new(ClaimTypes.GivenName, userResult.FirstName),
            new(ClaimTypes.Surname, userResult.LastName),
            new(ClaimTypes.Role, userResult.Role)
        };
        var accessToken = jwtManager.Generate(claims);
        var refreshToken = jwtManager.Generate(claims);
        // TODO implement real refresh token

        return new UserResponse
        {
            UserId = userResult.UserId,
            FirstName = userResult.FirstName,
            LastName = userResult.LastName,
            Email = userResult.Email,
            Role = userResult.Role,
            RefreshToken = refreshToken,
            AccessToken = accessToken
        };
    }
}