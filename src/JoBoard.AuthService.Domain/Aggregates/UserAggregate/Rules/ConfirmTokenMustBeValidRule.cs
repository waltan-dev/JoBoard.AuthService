using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ConfirmTokenMustBeValidRule : IBusinessRule
{
    private readonly ConfirmationToken _confirmationToken;
    private readonly string _requestToken;

    public ConfirmTokenMustBeValidRule(ConfirmationToken confirmationToken, string requestToken)
    {
        _confirmationToken = confirmationToken;
        _requestToken = requestToken;
    }
    
    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_requestToken)
               || _confirmationToken.Value != _requestToken 
               || DateTime.UtcNow > _confirmationToken.Expiration;
    }

    public string Message => "Invalid or expired token";
}