using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Unit>
{
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPasswordResetCommandHandler(
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RequestPasswordResetCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var email = new Email(request.Email);

        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new DomainException("User not found");

        var newToken = ConfirmationToken.Generate(_confirmationTokenConfig.ExpiresInHours);
        user.RequestPasswordReset(newToken);

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send email
        return Unit.Value;
    }
}