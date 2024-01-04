using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeRole;

public class ChangeRoleCommand : IRequest
{
    public Guid UserId { get; set; }
    public string NewRole { get; set; }
}