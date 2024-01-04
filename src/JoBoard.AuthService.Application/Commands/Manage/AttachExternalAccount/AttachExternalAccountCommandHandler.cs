using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.AttachExternalAccount;

public class AttachExternalAccountCommandHandler : IRequestHandler<AttachExternalAccountCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AttachExternalAccountCommandHandler(
        IIdentityService identityService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(AttachExternalAccountCommand request, CancellationToken ct)
    {
        var externalAccount = new ExternalNetworkAccount(request.ExternalUserId, request.ExternalNetwork);
        await CheckIfAlreadyExistsAsync(externalAccount, ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new DomainException("User not found");
        
        user.AttachNetwork(externalAccount);
        
        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    private async Task CheckIfAlreadyExistsAsync(ExternalNetworkAccount externalAccount, CancellationToken ct)
    {
        var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (existingUser != null)
            throw new DomainException("This external account is already in use");
    }
}