using System.Text.RegularExpressions;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class Email : ValueObject
{
    public Email(string value)
    {
        Validate(value);
        Value = value.Trim().ToLower();
    }

    public string Value { get; private set; }
    
    private static void Validate(string email)
    {
        DomainGuard.IsNotNullOrWhiteSpace(email);
        
        var isValid = Regex.IsMatch(email,
            @"(?!^[.+&'_-]{1,64}@.*$)(^[_\w\d+&'-]+(\.[_\w\d+&'-]{1,64})*@[^\W_]+(\.[\w\d-]{1,255})*\.(([\d]{1,3})|([^\W_]{2,}))$)");
        if(isValid == false)
            throw new DomainException("Invalid email address");
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}