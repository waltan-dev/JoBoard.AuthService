using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.DeactivateAccount.Request;

public class RequestAccountDeactivationCommandHandler : IRequestHandler<RequestAccountDeactivationCommand, Unit>
{
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestAccountDeactivationCommandHandler(
        ConfirmationTokenConfig confirmationTokenConfig,
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _confirmationTokenConfig = confirmationTokenConfig;
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RequestAccountDeactivationCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var token = ConfirmationToken.Generate(_confirmationTokenConfig.ExpiresInHours);
        user.RequestAccountDeactivation(token);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        return Unit.Value;
    }
}