using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLoginByGoogle;

public class CanLoginByGoogleAccountCommandHandler : IRequestHandler<CanLoginByGoogleAccountCommand, LoginResult>
{
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;

    public CanLoginByGoogleAccountCommandHandler(
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository)
    {
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
    }
    
    public async Task<LoginResult> Handle(CanLoginByGoogleAccountCommand request, CancellationToken ct)
    {
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccountValue(googleUserProfile.Id, ExternalAccountProvider.Google);
        var user = await _userRepository.FindByExternalAccountValueAsync(externalAccount, ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.CanLoginWithExternalAccount(externalAccount);
        
        return new LoginResult(
            user.Id.Value.ToString(),
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}