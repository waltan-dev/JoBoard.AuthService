using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.Services;

public interface ITokenizer
{
    public ConfirmationToken Generate(int? expiresInHours = null);
}