using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.AttachExternalAccount;

public class AttachExternalAccountCommandHandler : IRequestHandler<AttachExternalAccountCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AttachExternalAccountCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(AttachExternalAccountCommand request, CancellationToken ct)
    {
        var externalAccount = new ExternalNetworkAccount(request.ExternalUserId, request.ExternalNetwork);
        await CheckIfAlreadyExistsAsync(externalAccount, ct);
        
        var user = await _userRepository.FindByIdAsync(new UserId(request.UserId), ct);
        if (user == null)
            throw new DomainException("User not found");
        
        user.AttachNetwork(externalAccount);
        
        _userRepository.Update(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    private async Task CheckIfAlreadyExistsAsync(ExternalNetworkAccount externalAccount, CancellationToken ct)
    {
        var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (existingUser != null)
            throw new DomainException("This external account is already in use");
    }
}