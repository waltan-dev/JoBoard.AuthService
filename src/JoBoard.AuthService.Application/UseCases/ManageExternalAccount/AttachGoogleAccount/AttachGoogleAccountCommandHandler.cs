using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;
using ExternalAccountValue = JoBoard.AuthService.Domain.Aggregates.User.ValueObjects.ExternalAccountValue;

namespace JoBoard.AuthService.Application.UseCases.ManageExternalAccount.AttachGoogleAccount;

public class AttachGoogleAccountCommandHandler : IRequestHandler<AttachGoogleAccountCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;

    public AttachGoogleAccountCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IGoogleAuthProvider googleAuthProvider,
        IIdentityService identityService,
        IUserRepository userRepository)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _googleAuthProvider = googleAuthProvider;
        _identityService = identityService;
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(AttachGoogleAccountCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccountValue(googleUserProfile.Id, ExternalAccountProvider.Google);
        await CheckIfAlreadyExistsAsync(externalAccount, ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.AttachExternalAccount(externalAccount);
        
        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }

    private async Task CheckIfAlreadyExistsAsync(ExternalAccountValue externalAccount, CancellationToken ct)
    {
        var existingUser = await _userRepository.FindByExternalAccountValueAsync(externalAccount, ct);
        if (existingUser != null)
            throw new DomainException("This external account is already in use");
    }
}