﻿using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.ChangeEmail.Confirmation;

public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeEmailCommandHandler(
        IIdentityService identityService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ChangeEmailCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new DomainException("User not found");

        user.ChangeEmail(request.ConfirmationToken);

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}