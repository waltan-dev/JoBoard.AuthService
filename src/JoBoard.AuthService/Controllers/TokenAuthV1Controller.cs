using JoBoard.AuthService.Application.Commands.Account.Login.CanLoginByGoogle;
using JoBoard.AuthService.Application.Commands.Account.Login.CanLoginByPassword;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[AllowAnonymous]
[Produces("application/json")]
public class TokenAuthV1Controller : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtSignInManager _jwtSignInManager;

    public TokenAuthV1Controller(IMediator mediator,
        JwtSignInManager jwtSignInManager)
    {
        _mediator = mediator;
        _jwtSignInManager = jwtSignInManager;
    }
    
    [HttpPost(AuthTokenV1Routes.TokenByPassword)]
    public async Task<IActionResult> TokenByPassword(LoginByPasswordRequest request, CancellationToken ct)
    {
        var loginResult = await _mediator.Send(new CanLoginByPasswordCommand
        {
            Email = request.Email,
            Password = request.Password
        }, ct);
        return Ok(await _jwtSignInManager.SignInAsync(loginResult, ct));
    }
    
    [HttpPost(AuthTokenV1Routes.TokenByGoogle)]
    public async Task<IActionResult> TokenByGoogle(LoginByGoogleRequest request, CancellationToken ct)
    {
        var loginResult = await _mediator.Send(new CanLoginByGoogleAccountCommand
        {
            GoogleIdToken = request.IdToken
        }, ct);
        return Ok(await _jwtSignInManager.SignInAsync(loginResult, ct));
    }
    
    [HttpPost(AuthTokenV1Routes.RefreshToken)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken ct)
    {
        return Ok(await _jwtSignInManager.RefreshTokenAsync(request.ExpiredAccessToken, request.RefreshToken, ct));
    }
    
    [HttpPost(AuthTokenV1Routes.RevokeRefreshToken), Authorize]
    public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest request, CancellationToken ct)
    {
        await _jwtSignInManager.RevokeRefreshTokenAsync(request.RefreshToken, ct);
        return Ok();
    }
}