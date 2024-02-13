using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;

public class LoginByGoogleAccountCommandHandler : IRequestHandler<LoginByGoogleAccountCommand, UserResult>
{
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;

    public LoginByGoogleAccountCommandHandler(
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository)
    {
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
    }
    
    public async Task<UserResult> Handle(LoginByGoogleAccountCommand request, CancellationToken ct)
    {
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        var googleUserProfile = await _googleAuthProvider.VerifyIdTokenAsync(request.GoogleIdToken);
        if (googleUserProfile == null)
            throw new ValidationException(nameof(request.GoogleIdToken),"Google ID token isn't valid");
        
        var externalAccount = new ExternalAccount(googleUserProfile.Id, ExternalAccountProvider.Google);
        var user = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (user == null)
            throw new NotFoundException("User not found");
        user.ThrowIfBlockedOrDeactivated();
        
        return new UserResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}