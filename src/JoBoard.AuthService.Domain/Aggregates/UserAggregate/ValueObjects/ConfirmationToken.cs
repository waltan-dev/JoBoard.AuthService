﻿using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.Extensions;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

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
        DomainGuard.IsNotDefault(lifeSpan);
        DomainGuard.IsGreaterThan(lifeSpan, TimeSpan.Zero);
        
        var secureToken = secureTokenizer.Generate();
        return new ConfirmationToken(secureToken, DateTime.UtcNow.Add(lifeSpan).TrimMilliseconds());
    }
    
    public void Verify(string token, IDateTime dateTime)
    {
        CheckRule(new ConfirmTokenMustBeValidRule(this, token, dateTime));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}