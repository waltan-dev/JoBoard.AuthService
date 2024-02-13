using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JoBoard.AuthService.Application.Commands.Account.Login.CanLogin;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Models;
using JoBoard.AuthService.Infrastructure.Auth.Services;
using JoBoard.AuthService.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public class JwtSignInManager
{
    private readonly IMediator _mediator;
    private readonly JwtConfig _jwtConfig;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IIdentityService _identityService;

    public JwtSignInManager(IMediator mediator,
        JwtConfig jwtConfig, 
        ISecureTokenizer secureTokenizer, 
        IRefreshTokenRepository refreshTokenRepository,
        IIdentityService identityService)
    {
        _mediator = mediator;
        _jwtConfig = jwtConfig;
        _secureTokenizer = secureTokenizer;
        _refreshTokenRepository = refreshTokenRepository;
        _identityService = identityService;
    }

    public async Task<AuthResponse> SignInAsync(LoginResult loginResult, CancellationToken ct)
    {
        var accessToken = GenerateAccessToken(loginResult);
        var refreshToken = RefreshToken.Create(_jwtConfig.RefreshTokenLifeSpan, _secureTokenizer);
        await _refreshTokenRepository.AddTokenAsync(loginResult.UserId.ToString(), refreshToken, ct);
        
        return new AuthResponse(
            loginResult.UserId, 
            loginResult.FirstName, 
            loginResult.LastName, 
            loginResult.Email, 
            loginResult.Role, 
            accessToken, 
            refreshToken.Token);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        // validate access token
        var principal = GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
        var userId = principal?.GetUserId();
        if (principal == null || userId == null)
            throw new ValidationException(request.ExpiredAccessToken, "Access token isn't valid");
        
        var loginResult = await _mediator.Send(new CanLoginCommand { UserId = userId, }, ct);
        
        // validate refresh token
        var userRefreshTokens = await _refreshTokenRepository.GetTokensAsync(userId, ct);
        var currentRefreshToken = userRefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);
        if(currentRefreshToken == null)
            throw new ValidationException(request.RefreshToken, "Refresh token isn't valid or expired");
        if(DateTime.UtcNow > currentRefreshToken.ExpiresAt)
            throw new ValidationException(request.RefreshToken, "Refresh token is expired");
        
        // refresh
        var authResponse = await SignInAsync(loginResult, ct);
        await _refreshTokenRepository.RemoveTokenAsync(loginResult.UserId.ToString(), currentRefreshToken, ct);
        
        // remove expired tokens
        foreach (var expiredToken in userRefreshTokens.Where(x=> DateTime.UtcNow > x.ExpiresAt))
            await _refreshTokenRepository.RemoveTokenAsync(loginResult.UserId.ToString(), expiredToken, ct);
        
        return authResponse;
    }

    public async Task RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken ct)
    {
        var userId = _identityService.GetUserId();
        var userRefreshTokens = await _refreshTokenRepository.GetTokensAsync(userId, ct);
        var refreshToken = userRefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);
        if (refreshToken == null)
            throw new NotFoundException("Refresh token not found");
        
        await _refreshTokenRepository.RemoveTokenAsync(userId, refreshToken, ct);
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string expiredAccessToken)
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

    private string GenerateAccessToken(LoginResult loginResult)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResult.UserId.ToString()),
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.GivenName, loginResult.FirstName),
            new(ClaimTypes.Surname, loginResult.LastName),
            new(ClaimTypes.Role, loginResult.Role)
        };
        
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