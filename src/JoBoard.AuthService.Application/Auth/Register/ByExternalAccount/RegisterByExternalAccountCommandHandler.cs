using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Register.ByExternalAccount;

public class RegisterByExternalAccountCommandHandler : IRequestHandler<RegisterByExternalAccountCommand, Unit>
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
    
    public async Task<Unit> Handle(RegisterByExternalAccountCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var externalAccount = new ExternalAccount(request.ExternalUserId, request.ExternalAccountProvider);
        var existingUser = await _userRepository.FindByExternalAccountAsync(externalAccount, ct);
        if (existingUser != null) // user is already registered
            return Unit.Value;
        
        // register new user
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newUser = new User(
            userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            externalAccount: externalAccount,
            registerConfirmToken: _tokenizer.Generate());

        await _userRepository.AddAsync(newUser, ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send confirmation email
        return Unit.Value;
    }
}