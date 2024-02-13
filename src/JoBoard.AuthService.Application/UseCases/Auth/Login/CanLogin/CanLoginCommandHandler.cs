using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.Common.Models;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.CanLogin;

public class CanLoginCommandHandler : IRequestHandler<CanLoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;

    public CanLoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<LoginResult> Handle(CanLoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FindByIdAsync(new UserId(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        user.CanLogin();
        
        return new LoginResult(
            user.Id.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}