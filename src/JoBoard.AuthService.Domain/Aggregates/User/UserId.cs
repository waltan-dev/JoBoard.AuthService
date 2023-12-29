using Ardalis.GuardClauses;
using JoBoard.AuthService.Domain.Core;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class UserId : ValueObject
{
    public Guid Value { get; }
    
    public UserId(Guid value)
    {
        Guard.Against.Default(value);
        Value = value;
    }

    public static UserId Generate()
    {
        return new UserId(Guid.NewGuid());
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}