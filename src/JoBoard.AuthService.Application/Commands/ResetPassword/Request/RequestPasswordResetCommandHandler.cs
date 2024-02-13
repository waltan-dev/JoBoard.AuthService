using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Request;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Unit>
{
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IDateTime _dateTime;

    public RequestPasswordResetCommandHandler(
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IDateTime dateTime)
    {
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(RequestPasswordResetCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        user.RequestPasswordReset(newToken, _dateTime);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        // TODO send email
        return Unit.Value;
    }
}