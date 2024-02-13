using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;

public class LoginByGoogleAccountCommandHandler : IRequestHandler<LoginByGoogleAccountCommand, UserResult>
{
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginByGoogleAccountCommandHandler(
        IGoogleAuthProvider googleAuthProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
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
        
        return new UserResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}