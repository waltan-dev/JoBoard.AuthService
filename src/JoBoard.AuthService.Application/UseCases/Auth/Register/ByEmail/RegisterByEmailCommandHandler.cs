using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;

public class RegisterByEmailCommandHandler : IRequestHandler<RegisterByEmailCommand, UserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ConfirmationTokenConfig _confirmationTokenConfig;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByEmailCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ConfirmationTokenConfig confirmationTokenConfig,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _confirmationTokenConfig = confirmationTokenConfig;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResult> Handle(RegisterByEmailCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newToken = ConfirmationToken.Generate(_confirmationTokenConfig.ExpiresInHours);
        var newUser = new User(
            userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            passwordHash: _passwordHasher.Hash(request.Password),
            registerConfirmToken: newToken);

        await _userRepository.AddAsync(newUser, ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send confirmation email
        return new UserResult(
            newUser.Id.Value,
            newUser.FullName.FirstName,
            newUser.FullName.LastName,
            newUser.Email.Value,
            newUser.Role.Name);
    }
}