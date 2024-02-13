using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate;

public interface IUserEmailUniquenessChecker
{
    bool IsUnique(Email email);
}