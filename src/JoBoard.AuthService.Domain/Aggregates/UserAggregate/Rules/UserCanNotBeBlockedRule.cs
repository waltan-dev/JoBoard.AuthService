using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserCanNotBeBlockedRule : IBusinessRule
{
    private readonly UserStatus _status;

    public UserCanNotBeBlockedRule(UserStatus status)
    {
        _status = status;
    }
    
    public bool IsBroken()
    {
        return _status.Equals(UserStatus.Blocked);
    }

    public string Message => "Your account has been blocked";
}