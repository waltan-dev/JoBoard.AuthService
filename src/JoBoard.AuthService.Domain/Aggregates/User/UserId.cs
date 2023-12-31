using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class UserId : ValueObject
{
    public Guid Value { get; }
    
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