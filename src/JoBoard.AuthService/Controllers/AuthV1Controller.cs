using System.Security.Claims;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Application.UseCases.Auth.ConfirmEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
public class AuthV1Controller : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthV1Controller(IMediator mediator, IJwtGenerator jwtGenerator)
    {
        _mediator = mediator;
        _jwtGenerator = jwtGenerator;
    }
    
    [HttpPost(AuthV1Routes.Login)]
    public async Task<IActionResult> Login(LoginByEmailCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(BuildAuthResponse(userResult));
    }
    
    [HttpPost(AuthV1Routes.LoginByGoogle)]
    public async Task<IActionResult> LoginByGoogle(LoginByGoogleAccountCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(BuildAuthResponse(userResult));
    }
    
    [HttpPost(AuthV1Routes.Register)]
    public async Task<IActionResult> Register(RegisterByEmailCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(BuildAuthResponse(userResult));
    }
    
    [HttpPost(AuthV1Routes.RegisterByGoogle)]
    public async Task<IActionResult> RegisterByGoogle(RegisterByGoogleAccountCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(BuildAuthResponse(userResult));
    }
    
    [HttpPost(AuthV1Routes.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(AuthV1Routes.RequestPasswordReset)]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(AuthV1Routes.ConfirmPasswordReset)]
    public async Task<IActionResult> ConfirmPasswordReset(ConfirmPasswordResetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }

    private AuthResponse BuildAuthResponse(UserResult userResult)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userResult.UserId.ToString()),
            new(ClaimTypes.Email, userResult.Email),
            new(ClaimTypes.GivenName, userResult.FirstName),
            new(ClaimTypes.Surname, userResult.LastName),
            new(ClaimTypes.Role, userResult.Role)
        };
        var accessToken = _jwtGenerator.GenerateAccessToken(claims);
        var refreshToken = _jwtGenerator.GenerateRefreshToken(claims);

        return new AuthResponse(
            userResult.UserId,
            userResult.FirstName,
            userResult.LastName,
            userResult.Email,
            userResult.Role,
            accessToken,
            refreshToken);
    }
}