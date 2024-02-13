using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ExternalAccountMustBelongToUserRule : IBusinessRule
{
    private readonly List<ExternalAccount> _userExternalAccounts;
    private readonly ExternalAccountValue _externalAccountValue;

    public ExternalAccountMustBelongToUserRule(List<ExternalAccount> userExternalAccounts, ExternalAccountValue externalAccountValue)
    {
        _userExternalAccounts = userExternalAccounts;
        _externalAccountValue = externalAccountValue;
    }
    
    public bool IsBroken()
    {
        return _userExternalAccounts
            .Select(x => x.Value)
            .Contains(_externalAccountValue) == false;
    }

    public string Message => "Unknown external account";
}