using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Models;
using JoBoard.AuthService.Infrastructure.Auth.Services;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Models;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Auth.Jwt;

public class JwtSignInManager
{
    private readonly JwtConfig _jwtConfig;
    private readonly RefreshTokenConfig _refreshTokenConfig;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IRefreshTokenStorage _refreshTokenStorage;

    public JwtSignInManager(JwtConfig jwtConfig, 
        RefreshTokenConfig refreshTokenConfig,
        ISecureTokenizer secureTokenizer, 
        IRefreshTokenStorage refreshTokenStorage)
    {
        _jwtConfig = jwtConfig;
        _refreshTokenConfig = refreshTokenConfig;
        _secureTokenizer = secureTokenizer;
        _refreshTokenStorage = refreshTokenStorage;
    }

    public async Task<AuthResponse> SignInAsync(UserResult userResult)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userResult.UserId.ToString()),
            new(ClaimTypes.Email, userResult.Email),
            new(ClaimTypes.GivenName, userResult.FirstName),
            new(ClaimTypes.Surname, userResult.LastName),
            new(ClaimTypes.Role, userResult.Role)
        };

        var accessToken = GenerateAccessToken(claims);
        var refreshToken = RefreshToken.Create(_refreshTokenConfig.ExpiresInHours, _secureTokenizer);

        await _refreshTokenStorage.AddTokenAsync(userResult.UserId.ToString(), refreshToken);
        
        return new AuthResponse(
            userResult.UserId, 
            userResult.FirstName, 
            userResult.LastName, 
            userResult.Email, 
            userResult.Role, 
            accessToken, 
            refreshToken.Token);
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
}