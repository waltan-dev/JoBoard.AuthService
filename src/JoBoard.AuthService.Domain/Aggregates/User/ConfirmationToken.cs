using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

/// <summary>
/// Token for email confirmation or password recovering
/// </summary>
public class ConfirmationToken : ValueObject
{
    public string Value { get; }
    public DateTime Expiration { get; }
    
    public ConfirmationToken(string value, DateTime expiration)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Guard.IsNotDefault(expiration);
        Guard.IsGreaterThan(expiration, DateTime.UtcNow);
        
        Value = value;
        Expiration = expiration;
    }
    
    public bool Verify(string token, DateTime? dateTimeNow = null)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        
        dateTimeNow ??= DateTime.UtcNow;
        return Value == token && dateTimeNow <= Expiration;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}