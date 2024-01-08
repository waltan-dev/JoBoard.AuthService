using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application.Core.Services;

public interface IIdentityService
{
    public UserId GetUserId();
}