using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ConfirmTokenMustBeRequestedFirstRule : IBusinessRule
{
    private readonly ConfirmationToken? _confirmationToken;

    public ConfirmTokenMustBeRequestedFirstRule(ConfirmationToken? confirmationToken)
    {
        _confirmationToken = confirmationToken;
    }
    
    public bool IsBroken()
    {
        return _confirmationToken == null;
    }

    public string Message => "This operation has not been requested";
}