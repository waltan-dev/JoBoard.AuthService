using JoBoard.AuthService.Application.Models;

namespace JoBoard.AuthService.Models;

public class AuthResponse : LoginResult
{
    public AuthResponse(string userId, 
        string firstName, string lastName, 
        string email, string role, 
        string accessToken, string refreshToken) : base(userId, firstName, lastName, email, role)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}