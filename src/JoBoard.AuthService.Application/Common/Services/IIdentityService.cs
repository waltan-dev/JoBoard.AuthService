using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

namespace JoBoard.AuthService.Application.Common.Services;

public interface IIdentityService
{
    public UserId GetUserId();
}