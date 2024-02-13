using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserMustNotBeDeactivatedRule : IBusinessRule
{
    private readonly UserStatus _status;

    public UserMustNotBeDeactivatedRule(UserStatus status)
    {
        _status = status;
    }
    
    public bool IsBroken()
    {
        return _status.Equals(UserStatus.Deactivated);
    }

    public string Message => "Your account has been deactivated";
}