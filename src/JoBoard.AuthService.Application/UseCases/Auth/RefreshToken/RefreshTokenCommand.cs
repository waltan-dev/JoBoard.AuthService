using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenCommand : IRequest<Unit>
{
    public string RefreshToken { get; set; }
}