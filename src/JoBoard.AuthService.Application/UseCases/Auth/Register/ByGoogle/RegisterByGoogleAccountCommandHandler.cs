using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;

public class RegisterByGoogleAccountCommandHandler : IRequestHandler<RegisterByGoogleAccountCommand, UserResult>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByGoogleAccountCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResult> Handle(RegisterByGoogleAccountCommand request, CancellationToken ct)
    {
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccount(googleUserProfile.Id, ExternalAccountProvider.Google);
        var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (existingUser != null) // user is already registered 
            return new UserResult(
                existingUser.Id.Value,
                existingUser.FullName.FirstName,
                existingUser.FullName.LastName,
                existingUser.Email.Value,
                existingUser.Role.Name);
        
        // register new user
        var email = new Email(googleUserProfile.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");

        var newUser = User.RegisterByGoogleAccount(
            userId: UserId.Generate(),
            fullName: new FullName(googleUserProfile.FirstName, googleUserProfile.LastName),
            email: email,
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            googleUserId: googleUserProfile.Id);
        
        await _userRepository.AddAsync(newUser, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        return new UserResult(
            newUser.Id.Value,
            newUser.FullName.FirstName,
            newUser.FullName.LastName,
            newUser.Email.Value,
            newUser.Role.Name);
    }
}