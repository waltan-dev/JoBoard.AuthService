﻿using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ManageAccount.DeactivateAccount.Request;

public class RequestAccountDeactivationCommandHandler : IRequestHandler<RequestAccountDeactivationCommand, Unit>
{
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;

    public RequestAccountDeactivationCommandHandler(
        ConfirmationTokenConfig confirmationTokenConfig,
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        IUserRepository userRepository)
    {
        _confirmationTokenConfig = confirmationTokenConfig;
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(RequestAccountDeactivationCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var userId = UserId.FromValue(_identityService.GetUserId());
        var user = await _userRepository.FindByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var token = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        user.RequestAccountDeactivation(token);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}