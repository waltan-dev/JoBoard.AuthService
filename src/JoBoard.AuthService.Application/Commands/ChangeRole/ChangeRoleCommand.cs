using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeRole;

// immutable command
public class ChangeRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; init; }
    public string NewRole { get; init; }
}