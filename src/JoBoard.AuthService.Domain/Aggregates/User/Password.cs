using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class Password : ValueObject
{
    public string Hash { get; private set; }
    
    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password Create(string password, IPasswordStrengthValidator passwordStrengthValidator, IPasswordHasher passwordHasher)
    {
        if (passwordStrengthValidator.Validate(password) == false)
            throw new DomainException("Password is not strength");

        return new Password(passwordHasher.Hash(password));
    }

    public bool Verify(string passwordForCheck, IPasswordHasher passwordHasher)
    {
        return passwordHasher.Verify(Hash, passwordForCheck);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }
}