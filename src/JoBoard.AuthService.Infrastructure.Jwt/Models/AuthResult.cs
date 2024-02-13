using JoBoard.AuthService.Application.Models;

namespace JoBoard.AuthService.Infrastructure.Jwt.Models;

public class AuthResult : LoginResult
{
    public AuthResult() {}
    
    public AuthResult(string userId, string firstName, string lastName, string email, string role, string accessToken, string refreshToken) : base(userId, firstName, lastName, email, role)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}