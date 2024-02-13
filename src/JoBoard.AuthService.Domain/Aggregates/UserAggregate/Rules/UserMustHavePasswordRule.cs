using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserMustHavePasswordRule : IBusinessRule
{
    private readonly UserPassword? _passwordHash;

    public UserMustHavePasswordRule(UserPassword? passwordHash)
    {
        _passwordHash = passwordHash;
    }
    
    public bool IsBroken()
    {
        return _passwordHash == null;
    }

    public string Message => "No current password";
}