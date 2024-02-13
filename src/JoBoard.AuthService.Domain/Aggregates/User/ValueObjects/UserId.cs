using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }
    
    public UserId(Guid value)
    {
        Guard.IsNotDefault(value);
        Value = value;
    }

    public static UserId Generate()
    {
        return new UserId(Guid.NewGuid());
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}