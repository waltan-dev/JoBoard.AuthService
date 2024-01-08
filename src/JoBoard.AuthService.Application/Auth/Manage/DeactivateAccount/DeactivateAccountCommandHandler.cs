using JoBoard.AuthService.Application.Core.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.DeactivateAccount;

public class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, Unit>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateAccountCommandHandler(
        IIdentityService identityService,
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new DomainException("User not found");
        
        user.Deactivate();

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
        
        return Unit.Value;
    }
}