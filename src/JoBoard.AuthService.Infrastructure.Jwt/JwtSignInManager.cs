using System.Security.Claims;
using JoBoard.AuthService.Application.Commands.Login.CanLogin;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Infrastructure.Jwt.Extensions;
using JoBoard.AuthService.Infrastructure.Jwt.Models;
using JoBoard.AuthService.Infrastructure.Jwt.Services;
using MediatR;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public class JwtSignInManager
{
    private readonly IMediator _mediator;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IIdentityService _identityService;

    public JwtSignInManager(IMediator mediator,
        IJwtGenerator jwtGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IIdentityService identityService)
    {
        _mediator = mediator;
        _jwtGenerator = jwtGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _identityService = identityService;
    }
    
    public async Task<AuthInfo> SignInAsync(LoginResult loginResult, CancellationToken ct)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResult.UserId),
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.GivenName, loginResult.FirstName),
            new(ClaimTypes.Surname, loginResult.LastName),
            new(ClaimTypes.Role, loginResult.Role)
        };
        
        var accessToken = _jwtGenerator.GenerateAccessToken(claims);
        var refreshToken = _jwtGenerator.GenerateRefreshToken();
        await _refreshTokenRepository.AddTokenAsync(loginResult.UserId, refreshToken, ct);
        
        return new AuthInfo(
            loginResult.UserId, 
            loginResult.FirstName, 
            loginResult.LastName, 
            loginResult.Email, 
            loginResult.Role, 
            accessToken, 
            refreshToken.Token);
    }

    public async Task<AuthInfo> RefreshTokenAsync(string expiredAccessToken, string refreshToken, CancellationToken ct)
    {
        // validate access token
        var principal = _jwtGenerator.GetPrincipalFromExpiredToken(expiredAccessToken);
        var userId = principal?.GetUserId();
        if (principal == null || userId == null)
            throw new ValidationException(expiredAccessToken, "Access token isn't valid");
        
        // validate refresh token
        var userRefreshTokens = await _refreshTokenRepository.GetTokensAsync(userId, ct);
        var currentRefreshToken = userRefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
        if(currentRefreshToken == null)
            throw new ValidationException(refreshToken, "Refresh token isn't valid or expired");
        if(DateTime.UtcNow > currentRefreshToken.ExpiresAt)
            throw new ValidationException(refreshToken, "Refresh token is expired");
        
        // check user
        var loginResult = await _mediator.Send(new CanLoginCommand { UserId = userId, }, ct);
        
        // refresh token
        var authResponse = await SignInAsync(loginResult, ct); // creates new refresh token
        await _refreshTokenRepository.RemoveTokenAsync(loginResult.UserId, currentRefreshToken, ct);
        
        // remove expired tokens
        foreach (var expiredToken in userRefreshTokens.Where(x=> DateTime.UtcNow > x.ExpiresAt))
            await _refreshTokenRepository.RemoveTokenAsync(loginResult.UserId, expiredToken, ct);
        
        return authResponse;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        var userId = _identityService.GetUserId();
        var userRefreshTokens = await _refreshTokenRepository.GetTokensAsync(userId, ct);
        var userRefreshToken = userRefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
        if (userRefreshToken == null)
            throw new NotFoundException("Refresh token not found");
        
        await _refreshTokenRepository.RemoveTokenAsync(userId, userRefreshToken, ct);
    }
}