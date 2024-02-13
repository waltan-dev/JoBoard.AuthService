using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class ExternalAccountMustBeUniqueRule : IBusinessRule
{
    private readonly ExternalAccountValue _externalAccountValue;
    private readonly List<ExternalAccount>? _userExternalAccounts;
    private readonly IExternalAccountUniquenessChecker _externalAccountUniquenessChecker;

    public ExternalAccountMustBeUniqueRule(
        ExternalAccountValue externalAccountValue, 
        List<ExternalAccount>? userExternalAccounts,
        IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        _externalAccountValue = externalAccountValue;
        _userExternalAccounts = userExternalAccounts;
        _externalAccountUniquenessChecker = externalAccountUniquenessChecker;
    }
    
    public bool IsBroken()
    {
        return _userExternalAccounts?.Select(x=>x.Value).Contains(_externalAccountValue) == true
               ||  _externalAccountUniquenessChecker.IsUnique(_externalAccountValue) == false;
    }

    public string Message => "This external account is already in use";
}