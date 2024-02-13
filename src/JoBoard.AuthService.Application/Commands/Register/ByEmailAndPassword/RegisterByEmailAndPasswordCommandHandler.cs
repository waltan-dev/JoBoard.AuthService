using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Register.ByEmailAndPassword;

public class RegisterByEmailAndPasswordCommandHandler : IRequestHandler<RegisterByEmailAndPasswordCommand, Unit>
{
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IDateTime _dateTime;

    public RegisterByEmailAndPasswordCommandHandler(
        IPasswordStrengthValidator passwordStrengthValidator,
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUserEmailUniquenessChecker userEmailUniquenessChecker,
        ConfirmationTokenConfig confirmationTokenConfig,
        IDateTime dateTime)
    {
        _passwordStrengthValidator = passwordStrengthValidator;
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _confirmationTokenConfig = confirmationTokenConfig;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(RegisterByEmailAndPasswordCommand request, CancellationToken ct)
    {
        User newUser;
        try
        {
            var newPassword = UserPassword.Create(request.Password, _passwordStrengthValidator, _passwordHasher);
            newUser = User.RegisterByEmailAndPassword(
                userId: UserId.Generate(),
                fullName: new FullName(request.FirstName, request.LastName),
                email: new Email(request.Email),
                role: Enumeration.FromDisplayName<UserRole>(request.Role),
                password: newPassword,
                userEmailUniquenessChecker: _userEmailUniquenessChecker);
        }
        catch (DomainException ex)
        {
            if(ex.Message == UserEmailMustBeUniqueRule.ExceptionMessage)
                throw new ValidationException(nameof(request.Email),"This email is already in use");
            throw;
        }
        
        var emailConfirmationToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        newUser.RequestEmailConfirmation(emailConfirmationToken, _dateTime);
        
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        await _userRepository.AddAsync(newUser, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        // TODO send confirmation email
        return Unit.Value;
    }
}