using System.Reflection;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User.ValueObjects;

public class UserPasswordTests
{
    [Fact]
    public void CreateValid()
    {
        var passwordValue = PasswordFixtures.DefaultPassword;
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var passwordValidatorStub = UnitTestsRegistry.PasswordStrengthValidator;
        
        var exception = Record.Exception(() =>
        {
            var password = UserPassword.Create(passwordValue, passwordValidatorStub, passwordHasherStub);
            password.Verify(passwordValue, passwordHasherStub);
        });
        Assert.Null(exception);
    }
    
    [Fact]
    public void CreateNonStrength()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var passwordValidatorStub = UnitTestsRegistry.PasswordStrengthValidator;

        Assert.Throws<DomainException>(() =>
        {
            UserPassword.Create("weakPassword", passwordValidatorStub, passwordHasherStub);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var passwordHasherStub = UnitTestsRegistry.PasswordHasher;
        var hash = passwordHasherStub.Hash(PasswordFixtures.DefaultPassword);

        var password1 = CreateWithPrivateConstructor(hash);
        var password2 = CreateWithPrivateConstructor(hash);
        
        Assert.Equal(password1, password2);
    }
    
    private static UserPassword CreateWithPrivateConstructor(string hash)
    {
        Type type = typeof(UserPassword);
        var paramValues = new object[] { hash };  
        var paramTypes = new[] { typeof(string) };  
        var instance = type
            .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,null, paramTypes, null)!
            .Invoke(paramValues);  
        return (UserPassword)instance;
    }
}