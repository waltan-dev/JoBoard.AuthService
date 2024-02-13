using JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Confirmation;
using JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Request;
using JoBoard.AuthService.Application.UseCases.Manage.ChangePassword;
using JoBoard.AuthService.Application.UseCases.Manage.ChangeRole;
using JoBoard.AuthService.Application.UseCases.Manage.DeactivateAccount.Confirmation;
using JoBoard.AuthService.Application.UseCases.Manage.DeactivateAccount.Request;
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
    
    [HttpPost(ManageV1Routes.RequestEmailChange)]
    public async Task<IActionResult> RequestEmailChange(RequestEmailChangeCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(ManageV1Routes.ConfirmEmailChange)]
    public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(ManageV1Routes.ChangeRole)]
    public async Task<IActionResult> ChangeRole(ChangeRoleCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(ManageV1Routes.RequestAccountDeactivation)]
    public async Task<IActionResult> RequestAccountDeactivation(RequestAccountDeactivationCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
    
    [HttpPost(ManageV1Routes.ConfirmAccountDeactivation)]
    public async Task<IActionResult> ConfirmAccountDeactivation(ConfirmAccountDeactivationCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }
}