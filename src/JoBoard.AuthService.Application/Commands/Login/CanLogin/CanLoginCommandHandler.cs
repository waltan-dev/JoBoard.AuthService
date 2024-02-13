using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Application.Models;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using MediatR;

namespace JoBoard.AuthService.Application.Commands.Login.CanLogin;

public class CanLoginCommandHandler : IRequestHandler<CanLoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;

    public CanLoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<LoginResult> Handle(CanLoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FindByIdAsync(UserId.FromValue(request.UserId), ct);
        if (user == null)
            throw new NotFoundException("User not found");
        
        user.CanLogin();
        
        return new LoginResult(
            user.Id.Value.ToString(),
            user.FullName.FirstName,
            user.FullName.LastName,
            user.Email.Value,
            user.Role.Name);
    }
}