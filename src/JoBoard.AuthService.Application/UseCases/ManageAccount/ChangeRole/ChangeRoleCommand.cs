using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangeRole;

// immutable command
public class ChangeRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; init; }
    public string NewRole { get; init; }
}