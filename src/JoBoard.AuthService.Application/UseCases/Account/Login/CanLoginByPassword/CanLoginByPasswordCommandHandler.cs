﻿using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Login.CanLoginByPassword;

public class CanLoginByPasswordCommandHandler : IRequestHandler<CanLoginByPasswordCommand, LoginResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public CanLoginByPasswordCommandHandler(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    
    public async Task<LoginResult> Handle(CanLoginByPasswordCommand request, CancellationToken ct)
    {
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new DomainException("Invalid email or password");
        
        user.CanLoginWithPassword(request.Password, _passwordHasher);
        
        return new LoginResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}