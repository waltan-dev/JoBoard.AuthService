using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserEmailMustBeConfirmedRule : IBusinessRule
{
    private readonly UserStatus _status;
    private readonly bool _emailConfirmed;

    public UserEmailMustBeConfirmedRule(UserStatus status, bool emailConfirmed)
    {
        _status = status;
        _emailConfirmed = emailConfirmed;
    }
    
    public bool IsBroken()
    {
        return _status.Equals(UserStatus.Pending) || _emailConfirmed == false;
    }

    public string Message => "Email is not confirmed yet";
}