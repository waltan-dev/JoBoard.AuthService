using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.Services;

public interface IIdentityService
{
    public UserId GetUserId();
}