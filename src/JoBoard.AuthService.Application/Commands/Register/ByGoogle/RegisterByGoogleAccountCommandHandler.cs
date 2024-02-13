using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Register.ByGoogle;

public class RegisterByGoogleAccountCommandHandler : IRequestHandler<RegisterByGoogleAccountCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly IExternalAccountUniquenessChecker _externalAccountUniquenessChecker;

    public RegisterByGoogleAccountCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository,
        IUserEmailUniquenessChecker userEmailUniquenessChecker,
        IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _externalAccountUniquenessChecker = externalAccountUniquenessChecker;
    }
    
    public async Task<Unit> Handle(RegisterByGoogleAccountCommand request, CancellationToken ct)
    {
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        // register new user
        var newUser = User.RegisterByGoogleAccount(
            userId: UserId.Generate(),
            fullName: new FullName(googleUserProfile.FirstName, googleUserProfile.LastName),
            email: new Email(googleUserProfile.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            googleUserId: googleUserProfile.Id,
            _userEmailUniquenessChecker,
            _externalAccountUniquenessChecker);
        
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        await _userRepository.AddAsync(newUser, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);

        return Unit.Value;
    }
}