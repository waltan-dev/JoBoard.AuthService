using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserRoleMustBeWorkerOrHirerRule : IBusinessRule
{
    private readonly UserRole _userRole;

    public UserRoleMustBeWorkerOrHirerRule(UserRole userRole)
    {
        _userRole = userRole;
    }
    
    public bool IsBroken()
    {
        return _userRole.Equals(UserRole.Hirer) == false && _userRole.Equals(UserRole.Worker) == false;
    }

    public string Message => "Invalid role";
}