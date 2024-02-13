using JoBoard.AuthService.Application.Models;

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
}