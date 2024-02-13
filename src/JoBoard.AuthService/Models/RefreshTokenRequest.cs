namespace JoBoard.AuthService.Models;

public class RefreshTokenRequest
{
    public string ExpiredAccessToken { get; set; }
    public string RefreshToken { get; set; }
}