using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ManageExternalAccount.AttachGoogleAccount;

public class AttachGoogleAccountCommandHandler : IRequestHandler<AttachGoogleAccountCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IExternalAccountUniquenessChecker _externalAccountUniquenessChecker;

    public AttachGoogleAccountCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IGoogleAuthProvider googleAuthProvider,
        IIdentityService identityService,
        IUserRepository userRepository,
        IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _googleAuthProvider = googleAuthProvider;
        _identityService = identityService;
        _userRepository = userRepository;
        _externalAccountUniquenessChecker = externalAccountUniquenessChecker;
    }
    
    public async Task<Unit> Handle(AttachGoogleAccountCommand request, CancellationToken ct)
    {
        var userId = UserId.FromValue(_identityService.GetUserId());
        var user = await _userRepository.FindByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccountValue(googleUserProfile.Id, ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount, _externalAccountUniquenessChecker);
        
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}