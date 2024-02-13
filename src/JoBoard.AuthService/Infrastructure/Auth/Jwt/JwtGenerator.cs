using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JoBoard.AuthService.Infrastructure.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Auth.Jwt;

public interface IJwtManager
{
    string Generate(IEnumerable<Claim> claims);
}

public class JwtManager : IJwtManager
{
    private readonly JwtConfig _jwtConfig;
    
    public JwtManager(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }
    
    public string Generate(IEnumerable<Claim> claims)
    {
        var exp = DateTime.UtcNow.Add(_jwtConfig.TokenLifeSpan);
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