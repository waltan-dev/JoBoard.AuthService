using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    
    public async Task Handle(LoginCommand request, CancellationToken ct)
    {
        var hash = _passwordHasher.Hash(request.Password);
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAndPasswordAsync(email, hash, ct);
        if(user == null)
            throw new ValidationException("Invalid email or password");
        user.CheckStatus();
    }
}