using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.ByEmail;

public class LoginByEmailCommandHandler : IRequestHandler<LoginByEmailCommand, UserResult>
{
    private readonly RefreshTokenConfig _refreshTokenConfig;
    private readonly ISecureTokenizer _secureTokenizer;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public LoginByEmailCommandHandler(
        RefreshTokenConfig refreshTokenConfig,
        ISecureTokenizer secureTokenizer,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _refreshTokenConfig = refreshTokenConfig;
        _secureTokenizer = secureTokenizer;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _domainEventDispatcher = domainEventDispatcher;
    }
    
    public async Task<UserResult> Handle(LoginByEmailCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        
        var email = new Email(request.Email);
        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        var refreshToken = user.LoginWithPassword(
            request.Password, _passwordHasher, _secureTokenizer, _refreshTokenConfig.ExpiresInHours);
        
        await _userRepository.UpdateAsync(user, ct);    
        
        await _domainEventDispatcher.DispatchAsync(ct);
        await _unitOfWork.CommitAsync(ct);
        
        return new UserResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name,
            refreshToken.Token);
    }
}