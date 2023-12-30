using Ardalis.GuardClauses;
using JoBoard.AuthService.Domain.Core;

namespace JoBoard.AuthService.Domain.Aggregates.User;

/// <summary>
/// Token for email confirmation or password recovering
/// </summary>
public class ConfirmationToken : ValueObject
{
    public string Value { get; }
    public DateTime Expiration { get; }
    
    private ConfirmationToken(string value, DateTime expiration)
    {
        Guard.Against.Default(value);
        Value = value;
        Expiration = expiration;
    }

    public static ConfirmationToken Generate(int expiresInHours)
    {
        Guard.Against.Default(expiresInHours);
        return new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(expiresInHours));
    }

    public bool IsValid(string token)
    {
        if (Value != token || DateTime.UtcNow > Expiration)
            return false;
        
        return true;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}