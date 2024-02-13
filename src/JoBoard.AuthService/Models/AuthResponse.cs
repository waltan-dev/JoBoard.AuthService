using System.Security.Claims;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Infrastructure.Jwt;

namespace JoBoard.AuthService.Models;

public class AuthResponse : UserResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    public AuthResponse(Guid userId, 
        string firstName, string lastName, 
        string email, 
        string role, 
        string accessToken, string refreshToken) : base(userId, firstName, lastName, email, role)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

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
        var accessToken = jwtGenerator.GenerateAccessToken(claims);
        var refreshToken = jwtGenerator.GenerateRefreshToken(claims);

        return new AuthResponse(
            userResult.UserId,
            userResult.FirstName,
            userResult.LastName,
            userResult.Email,
            userResult.Role,
            accessToken,
            refreshToken);
    }
}