using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class PasswordMustBeStrengthRule : IBusinessRule
{
    private readonly string _requestPassword;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;

    public PasswordMustBeStrengthRule(string requestPassword, IPasswordStrengthValidator passwordStrengthValidator)
    {
        _requestPassword = requestPassword;
        _passwordStrengthValidator = passwordStrengthValidator;
    }
    
    public bool IsBroken()
    {
        return _passwordStrengthValidator.Validate(_requestPassword) == false;
    }

    public string Message => "Password is not strength";
}