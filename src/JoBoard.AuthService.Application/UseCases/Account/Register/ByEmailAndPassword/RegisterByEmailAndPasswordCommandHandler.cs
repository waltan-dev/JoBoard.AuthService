using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Register.ByEmailAndPassword;

public class RegisterByEmailAndPasswordCommandHandler : IRequestHandler<RegisterByEmailAndPasswordCommand, Unit>
{
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;

    public RegisterByEmailAndPasswordCommandHandler(
        IPasswordStrengthValidator passwordStrengthValidator,
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ConfirmationTokenConfig confirmationTokenConfig)
    {
        _passwordStrengthValidator = passwordStrengthValidator;
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _confirmationTokenConfig = confirmationTokenConfig;
    }
    
    public async Task<Unit> Handle(RegisterByEmailAndPasswordCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new ValidationException(nameof(request.Email),"This email is already in use");
        
        var password = PasswordHash.Create(request.Password, _passwordStrengthValidator, _passwordHasher);
        var newUser = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            passwordHash: password);
        var emailConfirmationToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        newUser.RequestEmailConfirmation(emailConfirmationToken);
        
        await _userRepository.AddAsync(newUser, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        // TODO send confirmation email
        return Unit.Value;
    }
}