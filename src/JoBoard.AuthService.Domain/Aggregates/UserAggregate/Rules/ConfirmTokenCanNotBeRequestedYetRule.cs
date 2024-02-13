using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ConfirmTokenCanNotBeRequestedYetRule : IBusinessRule
{
    private readonly ConfirmationToken? _confirmationToken;
    private readonly IDateTime _dateTime;

    public ConfirmTokenCanNotBeRequestedYetRule(ConfirmationToken? confirmationToken, IDateTime dateTime)
    {
        _confirmationToken = confirmationToken;
        _dateTime = dateTime;
    }
    
    public bool IsBroken()
    {
        var utcNow = _dateTime?.UtcNow ?? DateTime.UtcNow;
        return _confirmationToken != null && _confirmationToken.Expiration > utcNow;
    }

    public string Message => "This operation has been request already";
}