using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserCanNotBeDeactivatedRule : IBusinessRule
{
    private readonly UserStatus _status;

    public UserCanNotBeDeactivatedRule(UserStatus status)
    {
        _status = status;
    }
    
    public bool IsBroken()
    {
        return _status.Equals(UserStatus.Deactivated);
    }

    public string Message => "Your account has been deactivated";
}