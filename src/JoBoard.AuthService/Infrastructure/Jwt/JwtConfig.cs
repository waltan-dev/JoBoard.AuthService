using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt;


public class JwtConfig
{
    public string Issuer { get; set; }
    public TimeSpan TokenLifeSpan { get; set; }
    public TimeSpan RefreshTokenLifeSpan { get; set; }
    public string Key { get; set; }
}