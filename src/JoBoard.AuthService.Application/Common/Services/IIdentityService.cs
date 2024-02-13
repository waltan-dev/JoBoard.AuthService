using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Application.Common.Services;

public interface IIdentityService
{
    public UserId GetUserId();
}