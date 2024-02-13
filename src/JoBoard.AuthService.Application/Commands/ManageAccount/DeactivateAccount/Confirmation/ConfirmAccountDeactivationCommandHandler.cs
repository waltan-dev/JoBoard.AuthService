using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ManageAccount.DeactivateAccount.Confirmation;

public class ConfirmAccountDeactivationCommandHandler : IRequestHandler<ConfirmAccountDeactivationCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;

    public ConfirmAccountDeactivationCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        IUserRepository userRepository)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(ConfirmAccountDeactivationCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var userId = UserId.FromValue(_identityService.GetUserId());
        var user = await _userRepository.FindByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        user.ConfirmAccountDeactivation(request.ConfirmationToken);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}