using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogleAccount;

public class RegisterByGoogleAccountCommandHandler : IRequestHandler<RegisterByGoogleAccountCommand, Unit>
{
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByGoogleAccountCommandHandler(
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RegisterByGoogleAccountCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        // https://developers.google.com/identity/sign-in/web/backend-auth
        
        // 1) TODO verify google id token
        //request.GoogleIdToken
        
        // TODO get external user id from google by auth code
        
        // 2) TODO check if the user is already in database - If so, establish an authenticated session for the user
        // 3) TODO create new user with information in the ID token payload, and establish a session for the user. 
        
        throw new Exception();
        
        // var externalAccount = new ExternalAccount(request.ExternalUserId, request.ExternalAccountProvider);
        // var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        // if (existingUser != null) // user is already registered
        //     return Unit.Value;
        //
        // // register new user
        // var email = new Email(request.Email);
        // var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        // if (emailIsUnique == false)
        //     throw new DomainException("This email is already in use");
        //
        // var newToken = ConfirmationToken.Generate(_confirmationTokenConfig.ExpiresInHours);
        // var newUser = new User(
        //     userId: UserId.Generate(),
        //     fullName: new FullName(request.FirstName, request.LastName),
        //     email: new Email(request.Email),
        //     role: Enumeration.FromDisplayName<UserRole>(request.Role),
        //     externalAccount: externalAccount,
        //     registerConfirmToken: newToken);
        //
        // await _userRepository.AddAsync(newUser, ct);
        // await _unitOfWork.CommitAsync(ct);
        //
        // // TODO send confirmation email
        // return Unit.Value;
    }
}