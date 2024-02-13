using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Jwt.Configs;
using JoBoard.AuthService.Infrastructure.Jwt.Models;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt.Services;

public interface IJwtGenerator
{
    RefreshToken GenerateRefreshToken();
    string GenerateAccessToken(IEnumerable<Claim> claims);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string expiredAccessToken);
}

public class JwtGenerator : IJwtGenerator
{
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly JwtConfig _jwtConfig;

    public JwtGenerator(
        ISecureTokenizer secureTokenizer,
        JwtConfig jwtConfig)
    {
        _secureTokenizer = secureTokenizer;
        _jwtConfig = jwtConfig;
    }

    public RefreshToken GenerateRefreshToken()
    {
        return RefreshToken.Create(_jwtConfig.RefreshTokenLifeSpan, _secureTokenizer);
    }
    
    public string GenerateAccessToken(IEnumerable<Claim> claims)
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
    
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string expiredAccessToken)
    {
        var key = _jwtConfig.GetSymmetricSecurityKey();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = false // don't validate lifetime of expired token
        };
        var principal = new JwtSecurityTokenHandler().ValidateToken(expiredAccessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken)
            return null;
        if (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase) == false)
            return null;
        
        return principal;
    }
}