using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

public class Email : ValueObject
{
    public Email(string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        
        if(IsValid(value) == false)
            throw new ArgumentException("Invalid email address");
        
        Value = value.Trim().ToLower();
    }

    public string Value { get; private set; }

    private static bool IsValid(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return string.Equals(addr.Address.ToLower().Trim(), email.ToLower().Trim(), 
                StringComparison.InvariantCultureIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}