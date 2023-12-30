using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace JoBoard.AuthService.Application.Commands.RegisterByExternalAccount;

public class RegisterByExternalAccountCommandHandler : IRequestHandler<RegisterByExternalAccountCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConfirmationEmailConfig _config;

    public RegisterByExternalAccountCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IOptions<ConfirmationEmailConfig> options)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _config = options.Value;
    }
    
    public async Task Handle(RegisterByExternalAccountCommand request, CancellationToken ct)
    {
        var externalAccount = new ExternalNetworkAccount(request.ExternalUserId, request.ExternalNetwork);
        var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (existingUser != null) // user is already registered => return auth info 
            return;
        
        // register new user
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newUser = new User(
            userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            accountType: request.AccountType,
            externalNetworkAccount: externalAccount,
            confirmationToken: ConfirmationToken.Generate(expiresInHours: _config.ExpiresInHours));

        _userRepository.Add(newUser, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        
        // TODO send confirmation email
    }
}