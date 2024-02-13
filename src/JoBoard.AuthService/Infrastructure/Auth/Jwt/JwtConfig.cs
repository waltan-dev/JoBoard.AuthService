using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Auth.Jwt;


public class JwtConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan TokenLifeSpan { get; set; }
    public string SecretKey { get; set; }
    public TimeSpan RefreshTokenLifeSpan { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}