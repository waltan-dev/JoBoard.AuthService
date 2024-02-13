using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ManageAccount.ChangeRole;

public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;

    public ChangeRoleCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(ChangeRoleCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var user = await _userRepository.FindByIdAsync(UserId.FromValue(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.ChangeRole(Enumeration.FromDisplayName<UserRole>(request.NewRole));

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}