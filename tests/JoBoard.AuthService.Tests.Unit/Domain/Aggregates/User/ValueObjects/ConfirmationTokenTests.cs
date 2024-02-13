using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var secureTokenizerStub = SecureTokenizerStubFactory.Create();
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));
        
        Assert.NotEmpty(newToken.Value);
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void VerifyValid()
    {
        var secureTokenizerStub = SecureTokenizerStubFactory.Create();
        var exception = Record.Exception(() 
            => ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24)));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var secureTokenizerStub = SecureTokenizerStubFactory.Create();
        var newToken = ConfirmationToken.Create(secureTokenizerStub, TimeSpan.FromHours(24));

        Assert.Throws<DomainException>(() =>
        {
            newToken.Verify("invalid-token");
        });
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var secureTokenizerStub = SecureTokenizerStubFactory.Create();
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
        var secureTokenizerStub = SecureTokenizerStubFactory.Create();
        var secureToken = secureTokenizerStub.Generate();
        var expires = DateTime.UtcNow.AddHours(24);

        var token1 = new ConfirmationToken(secureToken, expires);
        var token2 = new ConfirmationToken(secureToken, expires);
        
        Assert.Equal(token1, token2);
    }
}