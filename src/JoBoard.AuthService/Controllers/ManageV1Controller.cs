using JoBoard.AuthService.Application.UseCases.Manage.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
public class ManageV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public ManageV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost(ManageV1Routes.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
}