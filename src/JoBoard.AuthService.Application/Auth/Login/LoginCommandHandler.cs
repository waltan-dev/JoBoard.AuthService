using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Unit>
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
    
    public async Task<Unit> Handle(LoginCommand request, CancellationToken ct)
    {
        var hash = _passwordHasher.Hash(request.Password);
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAndPasswordAsync(email, hash, ct);
        if(user == null)
            throw new DomainException("Invalid email or password");
        user.CheckStatus();
        
        return Unit.Value;
    }
}