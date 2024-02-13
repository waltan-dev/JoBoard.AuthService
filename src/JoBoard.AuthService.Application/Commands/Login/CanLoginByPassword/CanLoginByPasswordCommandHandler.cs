using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLoginByPassword;

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
            user.Id.Value.ToString(),
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}