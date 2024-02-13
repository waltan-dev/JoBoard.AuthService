using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ConfirmTokenMustBeValidRule : IBusinessRule
{
    private readonly ConfirmationToken _confirmationToken;
    private readonly string _requestToken;
    private readonly IDateTime _dateTime;

    public ConfirmTokenMustBeValidRule(ConfirmationToken confirmationToken, string requestToken, IDateTime dateTime)
    {
        _confirmationToken = confirmationToken;
        _requestToken = requestToken;
        _dateTime = dateTime;
    }
    
    public bool IsBroken()
    {
        var utcNow = _dateTime?.UtcNow ?? DateTime.UtcNow;
        return string.IsNullOrWhiteSpace(_requestToken)
               || _confirmationToken.Value != _requestToken 
               || utcNow > _confirmationToken.Expiration;
    }

    public string Message => "Invalid or expired token";
}