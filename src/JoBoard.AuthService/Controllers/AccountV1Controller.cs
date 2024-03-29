﻿using JoBoard.AuthService.Application.Commands.ConfirmEmail;
using JoBoard.AuthService.Application.Commands.Register.ByEmailAndPassword;
using JoBoard.AuthService.Application.Commands.Register.ByGoogle;
using JoBoard.AuthService.Application.Commands.ResetPassword.Confirmation;
using JoBoard.AuthService.Application.Commands.ResetPassword.Request;
using JoBoard.AuthService.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[AllowAnonymous]
[Produces("application/json")]
public class AccountV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost(AccountV1Routes.Register)]
    public async Task<IActionResult> Register(RegisterByEmailAndPasswordCommand andPasswordCommand, CancellationToken ct)
    {
        await _mediator.Send(andPasswordCommand, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(AccountV1Routes.RegisterByGoogle)]
    public async Task<IActionResult> RegisterByGoogle(RegisterByGoogleAccountCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(AccountV1Routes.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(AccountV1Routes.RequestPasswordReset)]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(AccountV1Routes.ConfirmPasswordReset)]
    public async Task<IActionResult> ConfirmPasswordReset(ConfirmPasswordResetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
}