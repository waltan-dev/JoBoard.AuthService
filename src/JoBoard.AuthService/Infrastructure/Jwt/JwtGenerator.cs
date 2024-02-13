using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public interface IJwtGenerator
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken(IEnumerable<Claim> claims);
}

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfig _jwtConfig;
    
    public JwtGenerator(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_jwtConfig.TokenLifeSpan);
        return Generate(exp, claims);
    }

    public string GenerateRefreshToken(IEnumerable<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_jwtConfig.RefreshTokenLifeSpan);
        return Generate(exp, claims);
    }
    
    private string Generate(DateTime exp, IEnumerable<Claim> claims)
    {
        var key = _jwtConfig.GetSymmetricSecurityKey();
        var jwt = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: exp,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}