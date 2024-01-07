using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Register.ByEmail;

public class RegisterByEmailCommandHandler : IRequestHandler<RegisterByEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenizer _tokenizer;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterByEmailCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenizer tokenizer,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenizer = tokenizer;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RegisterByEmailCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var email = new Email(request.Email);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(email, ct);
        if (emailIsUnique == false)
            throw new DomainException("This email is already in use");
        
        var newUser = new User(
            userId: UserId.Generate(),
            fullName: new FullName(request.FirstName, request.LastName),
            email: new Email(request.Email),
            role: Enumeration.FromDisplayName<UserRole>(request.Role),
            passwordHash: _passwordHasher.Hash(request.Password),
            registerConfirmToken: _tokenizer.Generate());

        await _userRepository.AddAsync(newUser, ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send confirmation email
    }
}