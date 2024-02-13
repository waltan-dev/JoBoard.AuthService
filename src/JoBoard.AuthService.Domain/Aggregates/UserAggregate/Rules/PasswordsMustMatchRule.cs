using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class PasswordsMustMatchRule : IBusinessRule
{
    private readonly string _currentPasswordHash;
    private readonly string _requestPassword;
    private readonly IPasswordHasher _passwordHasher;

    public PasswordsMustMatchRule(string currentPasswordHash, string requestPassword, IPasswordHasher passwordHasher)
    {
        _currentPasswordHash = currentPasswordHash;
        _requestPassword = requestPassword;
        _passwordHasher = passwordHasher;
    }
    
    public bool IsBroken()
    {
        return _passwordHasher.Verify(_currentPasswordHash, _requestPassword) == false;
    }

    public string Message => "Invalid password";
}