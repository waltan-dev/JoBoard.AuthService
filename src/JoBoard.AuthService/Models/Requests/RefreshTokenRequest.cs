namespace JoBoard.AuthService.Models.Requests;

public class RefreshTokenRequest
{
    public string ExpiredAccessToken { get; set; }
    public string RefreshToken { get; set; }
}