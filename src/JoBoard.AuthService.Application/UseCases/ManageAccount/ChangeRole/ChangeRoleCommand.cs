using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangeRole;

public class ChangeRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewRole { get; set; }
}