using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application.Common.Services;

public interface IIdentityService
{
    public UserId GetUserId();
}