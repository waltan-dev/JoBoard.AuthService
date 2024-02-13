using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Login.CanLogin;

public class CanLoginCommand : IRequest<LoginResult>
{
    public Guid UserId { get; set; }
}