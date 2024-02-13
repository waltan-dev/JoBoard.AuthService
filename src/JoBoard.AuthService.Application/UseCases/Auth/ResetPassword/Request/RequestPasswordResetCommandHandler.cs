using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Unit>
{
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPasswordResetCommandHandler(
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RequestPasswordResetCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        user.RequestPasswordReset(newToken);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send email
        return Unit.Value;
    }
}