using JoBoard.AuthService.Application.UseCases.Auth.ConfirmEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;
using JoBoard.AuthService.Infrastructure.Auth.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[AllowAnonymous]
[Produces("application/json")]
public class AuthV1Controller : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtSignInManager _jwtSignInManager;

    public AuthV1Controller(IMediator mediator, JwtSignInManager jwtSignInManager)
    {
        _mediator = mediator;
        _jwtSignInManager = jwtSignInManager;
    }
    
    [HttpPost(AuthV1Routes.Login)]
    public async Task<IActionResult> Login(LoginByEmailCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(await _jwtSignInManager.SignInAsync(userResult));
    }
    
    [HttpPost(AuthV1Routes.LoginByGoogle)]
    public async Task<IActionResult> LoginByGoogle(LoginByGoogleAccountCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(await _jwtSignInManager.SignInAsync(userResult));
    }
    
    [HttpPost(AuthV1Routes.Register)]
    public async Task<IActionResult> Register(RegisterByEmailCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(await _jwtSignInManager.SignInAsync(userResult));
    }
    
    [HttpPost(AuthV1Routes.RegisterByGoogle)]
    public async Task<IActionResult> RegisterByGoogle(RegisterByGoogleAccountCommand command, CancellationToken ct)
    {
        var userResult = await _mediator.Send(command, ct);
        return Ok(await _jwtSignInManager.SignInAsync(userResult));
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
}