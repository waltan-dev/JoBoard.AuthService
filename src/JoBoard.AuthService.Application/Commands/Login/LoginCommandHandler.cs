using JoBoard.AuthService.Application.Accounts;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.User;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthInfo>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(
        ITokenGenerator tokenGenerator, 
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    
    public async Task<AuthInfo> Handle(LoginCommand request, CancellationToken ct)
    {
        var hash = _passwordHasher.Hash(request.Password);
        var email = new Email(request.Email);
        var user = await _userRepository.GetByEmailAndPasswordAsync(email, hash, ct);
        if(user == null)
            throw new ValidationException("Invalid email or password");
        user.CheckStatus();

        return AuthInfo.Create(user, _tokenGenerator);
    }
}