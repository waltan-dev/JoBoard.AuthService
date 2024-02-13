using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }
    
    private UserId(Guid value)
    {
        DomainGuard.IsNotDefault(value);
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
    
    public static UserId FromValue(string? value)
    {
        if (Guid.TryParse(value, out var guid) == false)
            throw new DomainException("Invalid user id");
        return FromValue(guid);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}