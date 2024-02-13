using JoBoard.AuthService.Application.Commands.ChangeEmail.Confirmation;
using JoBoard.AuthService.Application.Commands.ChangeEmail.Request;
using JoBoard.AuthService.Application.Commands.ChangePassword;
using JoBoard.AuthService.Application.Commands.ChangeRole;
using JoBoard.AuthService.Application.Commands.DeactivateAccount.Confirmation;
using JoBoard.AuthService.Application.Commands.DeactivateAccount.Request;
using JoBoard.AuthService.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
public class ManageAccountV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public ManageAccountV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost(ManageAccountV1Routes.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(ManageAccountV1Routes.RequestEmailChange)]
    public async Task<IActionResult> RequestEmailChange(RequestEmailChangeCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(ManageAccountV1Routes.ConfirmEmailChange)]
    public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(ManageAccountV1Routes.ChangeRole)]
    public async Task<IActionResult> ChangeRole(ChangeRoleCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(ManageAccountV1Routes.RequestAccountDeactivation)]
    public async Task<IActionResult> RequestAccountDeactivation(RequestAccountDeactivationCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
    
    [HttpPost(ManageAccountV1Routes.ConfirmAccountDeactivation)]
    public async Task<IActionResult> ConfirmAccountDeactivation(ConfirmAccountDeactivationCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new EmptyResponse());
    }
}