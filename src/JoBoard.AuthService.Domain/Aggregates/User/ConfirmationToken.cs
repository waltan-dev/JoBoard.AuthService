using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

/// <summary>
/// Token for email confirmation or password recovering
/// </summary>
public class ConfirmationToken : ValueObject
{
    public string Value { get; }
    public DateTime Expiration { get; }
    
    private ConfirmationToken() {} // for ef core only
    
    private ConfirmationToken(string token, int expiresInHours)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        Guard.IsNotDefault(expiresInHours);
        
        Value = token;
        Expiration = DateTime.UtcNow.AddHours(expiresInHours).TrimMilliseconds();
    }

    public static ConfirmationToken Create(ISecureTokenizer secureTokenizer, int expiresInHours)
    {
        var secureToken = secureTokenizer.Generate();
        return new ConfirmationToken(secureToken, expiresInHours);
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