using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Login.CanLogin;

public class CanLoginCommand : IRequest<LoginResult>
{
    public Guid UserId { get; set; }
}