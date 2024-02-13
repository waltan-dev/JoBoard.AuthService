using JoBoard.AuthService.Application.UseCases.Auth.ConfirmEmail;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
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
    
    [HttpPost("register-with-google")]
    public async Task<IActionResult> RegisterWithGoogle(RegisterByEmailCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status200OK);
    }
}