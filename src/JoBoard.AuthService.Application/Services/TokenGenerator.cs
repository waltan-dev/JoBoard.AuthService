using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.Extensions.Options;

namespace JoBoard.AuthService.Application.Services;

public interface ITokenGenerator
{
    AccessToken GenerateAccessToken(List<Claim> claims);
    AccessToken GenerateRefreshToken(List<Claim> claims);
}

public class TokenGenerator : ITokenGenerator
{
    private readonly JwtTokenOptions _options;
    
    public TokenGenerator(IOptions<JwtTokenOptions> options)
    {
        _options = options.Value;
    }

    public AccessToken GenerateAccessToken(List<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_options.TokenLifeSpan);
        string accessToken = Generate(exp, claims);
        return new AccessToken(accessToken, exp, GetUserId(claims));
    }

    public AccessToken GenerateRefreshToken(List<Claim> claims)
    {
        DateTime exp = DateTime.UtcNow.Add(_options.RefreshTokenLifeSpan);
        string refreshToken = Generate(exp, claims);
        return new AccessToken(refreshToken, exp, GetUserId(claims));
    }
    
    private string Generate(DateTime exp, IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: exp,
            signingCredentials: _options.SigningCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string GetUserId(IEnumerable<Claim> claims)
    {
        return claims.First(x => x.Type == nameof(UserId)).Value;
    }
}