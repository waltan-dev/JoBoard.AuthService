using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;

public interface IExternalAccountUniquenessChecker
{
    bool IsUnique(ExternalAccountValue externalAccountValue);
}