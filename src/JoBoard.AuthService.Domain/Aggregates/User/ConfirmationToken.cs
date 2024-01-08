﻿using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

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
        Guard.IsNotNullOrWhiteSpace(value);
        Guard.IsNotDefault(expiration);
        
        Value = value;
        Expiration = expiration;
    }

    public static ConfirmationToken Generate(int expiresInHours = 24)
    {
        return new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(expiresInHours));
    }
    
    public bool Verify(string token)
    {
        Guard.IsNotNullOrWhiteSpace(token);
        
        return Value == token && DateTime.UtcNow <= Expiration;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expiration;
    }
}