using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class UserPassword : ValueObject
{
    private string Hash { get; set; }
    
    private UserPassword() {} // for ef core only
    
    private UserPassword(string hash)
    {
        Hash = hash;
    }

    /// <summary>
    /// Creates only strength password throughout the application
    /// </summary>
    public static UserPassword Create(string password, 
        IPasswordStrengthValidator passwordStrengthValidator, IPasswordHasher passwordHasher)
    {
        CheckRule(new PasswordMustBeStrengthRule(password, passwordStrengthValidator));
        return new UserPassword(passwordHasher.Hash(password));
    }

    public void Verify(string requestPassword, IPasswordHasher passwordHasher)
    {
        CheckRule(new PasswordsMustMatchRule(Hash, requestPassword, passwordHasher));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }
}