using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;

public class LoginByGoogleAccountCommandHandler : IRequestHandler<LoginByGoogleAccountCommand, UserResult>
{
    private readonly RefreshTokenConfig _refreshTokenConfig;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public LoginByGoogleAccountCommandHandler(
        RefreshTokenConfig refreshTokenConfig,
        ISecureTokenizer secureTokenizer,
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _refreshTokenConfig = refreshTokenConfig;
        _secureTokenizer = secureTokenizer;
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _domainEventDispatcher = domainEventDispatcher;
    }
    
    public async Task<UserResult> Handle(LoginByGoogleAccountCommand request, CancellationToken ct)
    {
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccount(googleUserProfile.Id, ExternalAccountProvider.Google);
        var user = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        var refreshToken = user.LoginWithExternalAccount(_secureTokenizer, _refreshTokenConfig.ExpiresInHours);
        await _userRepository.UpdateAsync(user, ct);

        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        return new UserResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name,
            refreshToken.Token);
    }
}