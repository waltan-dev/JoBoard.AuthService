using JoBoard.AuthService.Application.Common.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.Login.CanLoginByGoogle;

public class CanLoginByGoogleAccountCommand : IRequest<LoginResult>
{
    public string GoogleIdToken { get; set; }
}