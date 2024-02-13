using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

/// <summary>
/// Token for email confirmation or password recovering
/// </summary>
public class ConfirmationToken : ValueObject
{
    public string Value { get; private set; }
    public DateTime Expiration { get; private set; }
    
    private ConfirmationToken() {} // for ef core only

    private ConfirmationToken(string token, DateTime expiration)
    {
        Value = token;
        Expiration = expiration;
    }

    public static ConfirmationToken Create(ISecureTokenizer secureTokenizer, TimeSpan lifeSpan)
    {
        Guard.IsNotDefault(lifeSpan);
        
        var secureToken = secureTokenizer.Generate();
        return new ConfirmationToken(secureToken, DateTime.UtcNow.Add(lifeSpan).TrimMilliseconds());
    }
    
    public void Verify(string token)
    {
        CheckRule(new ConfirmTokenMustBeValidRule(this, token));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}