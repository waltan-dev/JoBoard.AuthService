using JoBoard.AuthService.Domain.Common;
using JoBoard.AuthService.Domain.Core;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class Email : ValueObject
{
    public Email(string value)
    {
        if(IsValid(value) == false)
            throw new ArgumentException("Invalid email address");
        
        Value = value.Trim().ToLower();
    }

    public string Value { get; }

    private static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
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
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}