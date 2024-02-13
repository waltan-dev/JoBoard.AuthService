using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.DataFixtures;

using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class PasswordHashTests
{
    [Fact]
    public void CreateValid()
    {
        var passwordValue = PasswordFixtures.DefaultPassword;
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var passwordValidatorStub = PasswordStrengthValidatorStubFactory.Create();

        PasswordHash? passwordHash = null;
        var exception = Record.Exception(() =>
        {
            passwordHash = PasswordHash.Create(passwordValue, passwordValidatorStub, passwordHasherStub);
        });
        Assert.Null(exception);
        Assert.True(passwordHasherStub.Verify(passwordHash!.Value, passwordValue));
    }
    
    [Fact]
    public void CreateNonStrength()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var passwordValidatorStub = PasswordStrengthValidatorStubFactory.Create();

        Assert.Throws<DomainException>(() =>
        {
            PasswordHash.Create("weakPassword", passwordValidatorStub, passwordHasherStub);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var passwordHasherStub = PasswordHasherStubFactory.Create();
        var hash = passwordHasherStub.Hash(PasswordFixtures.DefaultPassword);

        var password1 = new PasswordHash(hash);
        var password2 = new PasswordHash(hash);
        
        Assert.Equal(password1, password2);
    }
}