using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.Exceptions;
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

    internal ConfirmationToken(string token, DateTime expiration)
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
        Guard.IsNotNullOrWhiteSpace(token);

        if (Value != token)
            throw new DomainException("Invalid token");
        
        if(DateTime.UtcNow >= Expiration)
            throw new DomainException("Token expired");
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}