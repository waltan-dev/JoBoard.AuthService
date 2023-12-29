using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.Common;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FindByConfirmationTokenAsync(request.Token, ct);
        if (user == null)
            throw new DomainException("Invalid token");
        
        user.ConfirmEmail(request.Token);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}