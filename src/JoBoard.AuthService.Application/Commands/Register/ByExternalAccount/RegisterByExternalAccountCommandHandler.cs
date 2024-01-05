using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Register.ByExternalAccount;

public class RegisterByExternalAccountCommandHandler : IRequestHandler<RegisterByExternalAccountCommand>
{
    private readonly ITokenizer _tokenizer;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByExternalAccountCommandHandler(
        ITokenizer tokenizer,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenizer = tokenizer;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RegisterByExternalAccountCommand request, CancellationToken ct)
    {
        var existingUser = await _userRepository.FindByExternalAccountAsync(request.ExternalUserId, request.ExternalNetwork, ct);
        if (existingUser != null) // user is already registered => return auth info 
            return;
        
        // register new user
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");

        var newUserId = UserId.Generate();
        var externalAccount = new ExternalNetworkAccount(newUserId, request.ExternalUserId, request.ExternalNetwork);
        var newUser = new User(
            userId: newUserId,
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            externalNetworkAccount: externalAccount,
            registerConfirmToken: _tokenizer.Generate());

        await _userRepository.AddAsync(newUser, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        
        // TODO send confirmation email
    }
}