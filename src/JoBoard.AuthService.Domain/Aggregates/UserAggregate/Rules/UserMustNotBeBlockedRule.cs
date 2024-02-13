using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserMustNotBeBlockedRule : IBusinessRule
{
    private readonly UserStatus _status;

    public UserMustNotBeBlockedRule(UserStatus status)
    {
        _status = status;
    }
    
    public bool IsBroken()
    {
        return _status.Equals(UserStatus.Blocked);
    }

    public string Message => "Your account has been blocked";
}