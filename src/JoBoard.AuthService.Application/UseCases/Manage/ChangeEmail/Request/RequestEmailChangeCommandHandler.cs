﻿using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Manage.ChangeEmail.Request;

public class RequestEmailChangeCommandHandler : IRequestHandler<RequestEmailChangeCommand, Unit>
{
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestEmailChangeCommandHandler(
        ISecureTokenizer secureTokenizer,
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _secureTokenizer = secureTokenizer;
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _confirmationTokenConfig = confirmationTokenConfig;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(RequestEmailChangeCommand requestEmail, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newEmail = new Email(requestEmail.NewEmail);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(newEmail, ct);
        if(emailIsUnique == false)
            throw new DomainException("Email is already in use");

        var newToken = ConfirmationToken.Create(_secureTokenizer, _confirmationTokenConfig.TokenLifeSpan);
        user.RequestEmailChange(newEmail, newToken);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send email
        return Unit.Value;
    }
}