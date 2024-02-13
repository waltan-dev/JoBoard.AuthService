using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }
    
    internal UserId(Guid value)
    {
        Guard.IsNotDefault(value);
        Value = value;
    }

    public static UserId Generate()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId FromValue(Guid value)
    {
        return new UserId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}