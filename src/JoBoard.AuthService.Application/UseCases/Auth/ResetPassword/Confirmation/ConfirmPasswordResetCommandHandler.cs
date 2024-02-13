using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;

public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ConfirmPasswordResetCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Unit> Handle(ConfirmPasswordResetCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var user = await _userRepository.FindByIdAsync(new UserId(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.ConfirmPasswordReset(request.ConfirmationToken, request.NewPassword, _passwordHasher);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        return Unit.Value;
    }
}