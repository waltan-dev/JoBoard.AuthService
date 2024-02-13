using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUserRepository _userRepository;
    private readonly IDateTime _dateTime;

    public ConfirmEmailCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IUserRepository userRepository,
        IDateTime dateTime)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _userRepository = userRepository;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var user = await _userRepository.FindByIdAsync(UserId.FromValue(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.ConfirmEmail(request.Token, _dateTime);
        
        await _userRepository.UpdateAsync(user, ct);

        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);

        return Unit.Value;
    }
}