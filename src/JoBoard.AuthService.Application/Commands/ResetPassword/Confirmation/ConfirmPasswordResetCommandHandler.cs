using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Confirmation;

public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTime _dateTime;

    public ConfirmPasswordResetCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IPasswordStrengthValidator passwordStrengthValidator,
        IPasswordHasher passwordHasher,
        IDateTime dateTime)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _passwordStrengthValidator = passwordStrengthValidator;
        _passwordHasher = passwordHasher;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(ConfirmPasswordResetCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var user = await _userRepository.FindByIdAsync(UserId.FromValue(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newPassword = UserPassword.Create(request.NewPassword, _passwordStrengthValidator, _passwordHasher);
        user.ConfirmPasswordReset(request.ConfirmationToken, newPassword, _dateTime);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}