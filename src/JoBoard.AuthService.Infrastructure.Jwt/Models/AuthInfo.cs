namespace JoBoard.AuthService.Infrastructure.Jwt.Models;

public class AuthInfo
{
    public AuthInfo() {}
    
    public AuthInfo(string userId, string firstName, string lastName, string email, string role, string accessToken, string refreshToken)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}