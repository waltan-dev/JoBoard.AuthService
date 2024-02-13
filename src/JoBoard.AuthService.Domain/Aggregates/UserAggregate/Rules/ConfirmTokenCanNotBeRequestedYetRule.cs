using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ConfirmTokenCanNotBeRequestedYetRule : IBusinessRule
{
    private readonly ConfirmationToken? _confirmationToken;

    public ConfirmTokenCanNotBeRequestedYetRule(ConfirmationToken? confirmationToken)
    {
        _confirmationToken = confirmationToken;
    }
    
    public bool IsBroken()
    {
        return _confirmationToken != null && _confirmationToken.Expiration > DateTime.UtcNow;
    }

    public string Message => "This operation has been request already";
}