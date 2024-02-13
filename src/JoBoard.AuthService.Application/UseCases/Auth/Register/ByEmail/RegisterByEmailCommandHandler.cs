using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;

public class RegisterByEmailCommandHandler : IRequestHandler<RegisterByEmailCommand, Unit>
{
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByEmailCommandHandler(
        IPasswordStrengthValidator passwordStrengthValidator,
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUnitOfWork unitOfWork)
    {
        _passwordStrengthValidator = passwordStrengthValidator;
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _confirmationTokenConfig = confirmationTokenConfig;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RegisterByEmailCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        var password = PasswordHash.Create(request.Password, _passwordStrengthValidator, _passwordHasher);
        var newUser = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            passwordHash: password,
            registerConfirmToken: newToken);
        
        await _userRepository.AddAsync(newUser, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send confirmation email
        return Unit.Value;
    }
}