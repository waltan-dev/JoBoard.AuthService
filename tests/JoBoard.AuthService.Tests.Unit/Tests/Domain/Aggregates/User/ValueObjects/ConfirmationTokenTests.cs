using System.Reflection;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User.ValueObjects;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));
        
        Assert.NotEmpty(newToken.Value);
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;

        Assert.Throws<DomainException>(() =>
        {
            ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(-1));
        });
    }
    
    [Fact]
    public void VerifyValid()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;
        var exception = Record.Exception(() 
            => ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24)));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));

        Assert.Throws<DomainException>(() =>
        {
            newToken.Verify("invalid-token", UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;
        var value = Guid.NewGuid().ToString();
        var confirmationToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));
        
        Assert.Throws<DomainException>(() =>
        {
            confirmationToken.Verify(value, UnitTestsRegistry.FutureDateTime);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var secureTokenizerStub = UnitTestsRegistry.SecureTokenizer;
        var secureToken = secureTokenizerStub.Generate();
        var expires = DateTime.UtcNow.AddHours(24);
        
        var token1 = CreateWithPrivateConstructor(secureToken, expires);
        var token2 = CreateWithPrivateConstructor(secureToken, expires);
        
        Assert.Equal(token1, token2);
    }

    private static ConfirmationToken CreateWithPrivateConstructor(string secureToken, DateTime expires)
    {
        Type type = typeof(ConfirmationToken);
        var paramValues = new object[] { secureToken, expires };  
        var paramTypes = new[] { typeof(string), typeof(DateTime) };  
        var instance = type
            .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,null, paramTypes, null)!
            .Invoke(paramValues);  
        return (ConfirmationToken)instance;
    }
}