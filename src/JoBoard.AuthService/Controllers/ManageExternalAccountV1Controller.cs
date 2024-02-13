using JoBoard.AuthService.Application.UseCases.ManageExternalAccount.AttachGoogleAccount;
using JoBoard.AuthService.Application.UseCases.ManageExternalAccount.DetachExternalAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
public class ManageExternalAccountV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public ManageExternalAccountV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost(ManageExternalAccountV1Routes.AttachGoogleAccount)]
    public async Task<IActionResult> AttachGoogleAccount(AttachGoogleAccountCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(ManageExternalAccountV1Routes.DetachExternalAccount)]
    public async Task<IActionResult> DetachExternalAccount(DetachExternalAccountCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
}