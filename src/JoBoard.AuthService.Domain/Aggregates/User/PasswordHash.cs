using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class PasswordHash : ValueObject
{
    public string Value { get; private set; }
    
    private PasswordHash() {} // for ef core only
    
    private PasswordHash(string value)
    {
        Value = value;
    }

    public static PasswordHash Create(string password, IPasswordStrengthValidator passwordStrengthValidator, IPasswordHasher passwordHasher)
    {
        if (passwordStrengthValidator.Validate(password) == false)
            throw new DomainException("Password is not strength");

        return new PasswordHash(passwordHasher.Hash(password));
    }

    public bool Verify(string passwordForCheck, IPasswordHasher passwordHasher)
    {
        return passwordHasher.Verify(Value, passwordForCheck);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}