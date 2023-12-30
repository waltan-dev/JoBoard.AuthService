using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Application;

public class JwtTokenOptions
{
    public string Issuer { get; set; }
    public TimeSpan TokenLifeSpan { get; set; }
    public TimeSpan RefreshTokenLifeSpan { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
}