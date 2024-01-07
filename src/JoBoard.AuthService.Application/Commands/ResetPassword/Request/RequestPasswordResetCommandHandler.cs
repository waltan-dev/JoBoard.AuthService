using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword.Request;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenizer _tokenizer;

    public RequestPasswordResetCommandHandler(
        ITokenizer tokenizer,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenizer = tokenizer;
    }
    
    public async Task Handle(RequestPasswordResetCommand request, CancellationToken ct)
    {
        await _unitOfWork.StartTransactionAsync(ct);
        
        var email = new Email(request.Email);

        var user = await _userRepository.FindByEmailAsync(email, ct);
        if (user == null)
            throw new DomainException("User not found");

        var confirmationToken = _tokenizer.Generate();
        user.RequestPasswordReset(confirmationToken);

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
        
        // TODO send email
    }
}