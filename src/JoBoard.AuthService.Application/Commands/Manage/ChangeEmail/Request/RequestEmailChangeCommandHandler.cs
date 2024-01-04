using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Manage.ChangeEmail.Request;

public class RequestEmailChangeCommandHandler : IRequestHandler<RequestEmailChangeCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenizer _tokenizer;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestEmailChangeCommandHandler(
        IIdentityService identityService,
        ITokenizer tokenizer,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _tokenizer = tokenizer;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RequestEmailChangeCommand requestEmail, CancellationToken ct)
    {
        var user = await _userRepository.FindByIdAsync(_identityService.GetUserId(), ct);
        if (user == null)
            throw new DomainException("User not found");

        var newEmail = new Email(requestEmail.NewEmail);
        var emailIsUnique = await _userRepository.CheckEmailUniquenessAsync(newEmail, ct);
        if(emailIsUnique == false)
            throw new DomainException("Email is already in use");

        var confirmationToken = _tokenizer.Generate();
        user.RequestEmailChange(newEmail, confirmationToken);

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        
        // TODO send email
    }
}