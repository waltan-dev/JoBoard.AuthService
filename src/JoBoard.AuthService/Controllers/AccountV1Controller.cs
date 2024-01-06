﻿using System.Net;
using JoBoard.AuthService.Application.Commands.Register.ByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterByEmailCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created);
    }
}