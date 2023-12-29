using Ardalis.GuardClauses;
using JoBoard.AuthService.Domain.Core;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ConfirmationToken : ValueObject
{
    public string Value { get; }
    public DateTime Expiration { get; }
    
    private ConfirmationToken(string value, DateTime expiration)
    {
        Guard.Against.Default(value);
        if (expiration < DateTime.UtcNow)
            throw new ArgumentException("Expiration must be a date in the future");
        
        Value = value;
        Expiration = expiration;
    }

    public static ConfirmationToken Generate(uint expiresInHours)
    {
        Guard.Against.Default(expiresInHours);
        return new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(expiresInHours));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}