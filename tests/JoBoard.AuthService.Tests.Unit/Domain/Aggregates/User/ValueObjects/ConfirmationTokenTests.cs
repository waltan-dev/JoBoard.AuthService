using System.Reflection;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var secureTokenizerStub = TestsRegistry.SecureTokenizer;
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));
        
        Assert.NotEmpty(newToken.Value);
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void VerifyValid()
    {
        var secureTokenizerStub = TestsRegistry.SecureTokenizer;
        var exception = Record.Exception(() 
            => ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24)));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var secureTokenizerStub = TestsRegistry.SecureTokenizer;
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));

        Assert.Throws<DomainException>(() =>
        {
            newToken.Verify("invalid-token");
        });
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var secureTokenizerStub = TestsRegistry.SecureTokenizer;
        var value = Guid.NewGuid().ToString();
        var expiredConfirmationToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(-1));
        
        Assert.Throws<DomainException>(() =>
        {
            expiredConfirmationToken.Verify(value);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var secureTokenizerStub = TestsRegistry.SecureTokenizer;
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