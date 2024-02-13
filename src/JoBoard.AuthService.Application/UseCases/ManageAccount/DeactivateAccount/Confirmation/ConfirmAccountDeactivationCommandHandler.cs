using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.DeactivateAccount.Confirmation;

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
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new NotFoundException("User not found");

        user.ConfirmAccountDeactivation(request.ConfirmationToken);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}