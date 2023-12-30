using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public interface IJwtGenerator
{
    AccessToken GenerateAccessToken(List<Claim> claims);
    AccessToken GenerateRefreshToken(List<Claim> claims);
}

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfig _config;
    
    public JwtGenerator(IOptions<JwtConfig> options)
    {
        _config = options.Value;
    }

    public AccessToken GenerateAccessToken(List<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_config.TokenLifeSpan);
        string accessToken = Generate(exp, claims);
        return new AccessToken(accessToken, exp, GetUserId(claims));
    }

    public AccessToken GenerateRefreshToken(List<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_config.RefreshTokenLifeSpan);
        string refreshToken = Generate(exp, claims);
        return new AccessToken(refreshToken, exp, GetUserId(claims));
    }
    
    private string Generate(DateTime exp, IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var jwt = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: exp,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string GetUserId(IEnumerable<Claim> claims)
    {
        return claims.First(x => x.Type == nameof(AccessToken.UserId)).Value;
    }
}