namespace JoBoard.AuthService.Models.Requests;

public class RevokeRefreshTokenRequest
{
    public string RefreshToken { get; set; }
}