﻿using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.ChangeRole;

public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRoleCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(ChangeRoleCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var user = await _userRepository.FindByIdAsync(new UserId(request.UserId), ct);
        if (user == null)
            throw new DomainException("User not found");
        
        user.ChangeRole(Enumeration.FromDisplayName<UserRole>(request.NewRole));

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
        
        return Unit.Value;
    }
}