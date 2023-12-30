using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace JoBoard.AuthService.Application.Commands.RegisterByEmail;

public class RegisterByEmailCommandHandler : IRequestHandler<RegisterByEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConfirmationEmailConfig _config;

    public RegisterByEmailCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IOptions<ConfirmationEmailConfig> options)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _config = options.Value;
    }
    
    public async Task Handle(RegisterByEmailCommand request, CancellationToken ct)
    {
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newUser = new User(
            userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            accountType: request.AccountType,
            passwordHash: _passwordHasher.Hash(request.Password),
            confirmationToken: ConfirmationToken.Generate(expiresInHours: _config.ExpiresInHours));

        _userRepository.Add(newUser, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        
        // TODO send confirmation email
    }
}