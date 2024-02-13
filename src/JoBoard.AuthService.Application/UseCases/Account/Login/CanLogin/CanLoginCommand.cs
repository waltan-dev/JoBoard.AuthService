using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Login.CanLogin;

// immutable command
public class CanLoginCommand : IRequest<LoginResult>
{
    public Guid UserId { get; init; }
}