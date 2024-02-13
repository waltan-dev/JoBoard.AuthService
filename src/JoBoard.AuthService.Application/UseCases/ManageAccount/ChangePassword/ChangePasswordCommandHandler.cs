﻿using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.ManageAccount.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IIdentityService _identityService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly IUserRepository _userRepository;

    public ChangePasswordCommandHandler(
        IDomainEventDispatcher domainEventDispatcher,
        IIdentityService identityService,
        IPasswordHasher passwordHasher,
        IPasswordStrengthValidator passwordStrengthValidator,
        IUserRepository userRepository)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _identityService = identityService;
        _passwordHasher = passwordHasher;
        _passwordStrengthValidator = passwordStrengthValidator;
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        await _userRepository.UnitOfWork.BeginTransactionAsync(ct: ct);
        
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var newPassword = PasswordHash.Create(request.NewPassword, _passwordStrengthValidator, _passwordHasher);
        user.ChangePassword(request.CurrentPassword, newPassword, _passwordHasher);

        await _userRepository.UpdateAsync(user, ct);
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _userRepository.UnitOfWork.CommitTransactionAsync(ct);
        
        return Unit.Value;
    }
}