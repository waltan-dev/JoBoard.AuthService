using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate;

public interface IExternalAccountUniquenessChecker
{
    bool IsUnique(ExternalAccountValue externalAccountValue);
}