using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class PasswordHashTests
{
    [Fact]
    public void CreateValid()
    {
        var passwordValue = PasswordFixtures.DefaultPassword;
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var passwordValidator = PasswordFixtures.GetPasswordStrengthValidatorStub();

        PasswordHash? passwordHash = null;
        var exception = Record.Exception(() =>
        {
            passwordHash = PasswordHash.Create(passwordValue, passwordValidator, passwordHasher);
        });
        Assert.Null(exception);
        Assert.True(passwordHasher.Verify(passwordHash!.Value, passwordValue));
    }
    
    [Fact]
    public void CreateNonStrength()
    {
        var passwordHasher = PasswordFixtures.GetPasswordHasherStub();
        var passwordValidator = PasswordFixtures.GetPasswordStrengthValidatorStub();

        Assert.Throws<DomainException>(() =>
        {
            PasswordHash.Create("weakPassword", passwordValidator, passwordHasher);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var hash = PasswordFixtures.GetPasswordHasherStub().Hash(PasswordFixtures.DefaultPassword);

        var password1 = new PasswordHash(hash);
        var password2 = new PasswordHash(hash);
        
        Assert.Equal(password1, password2);
    }
}