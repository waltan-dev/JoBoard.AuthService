using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeEmail.Request;

public class RequestEmailChangeCommandHandler : IRequestHandler<RequestEmailChangeCommand, Unit>
{
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly IDateTime _dateTime;

    public RequestEmailChangeCommandHandler(
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IUserEmailUniquenessChecker userEmailUniquenessChecker,
        IDateTime dateTime)
    {
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(RequestEmailChangeCommand command, CancellationToken ct)
    {
        var userId = UserId.FromValue(_identityService.GetUserId());
        var user = await _userRepository.FindByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newEmail = new Email(command.NewEmail);
        var newToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        try
        {
            user.RequestEmailChange(newEmail, newToken, _userEmailUniquenessChecker, _dateTime);
        }
        catch (DomainException ex)
        {
            if (ex.Message == UserEmailMustBeUniqueRule.ExceptionMessage)
                throw new ValidationException(nameof(command.NewEmail), ex.Message);
            throw;
        }
        
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        // TODO send email
        return Unit.Value;
    }
}