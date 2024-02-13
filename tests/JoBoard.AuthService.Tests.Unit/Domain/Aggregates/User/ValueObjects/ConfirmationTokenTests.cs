using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var newToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), TimeSpan.FromHours(24));
        
        Assert.NotEmpty(newToken.Value);
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void VerifyValid()
    {
        var exception = Record.Exception(() 
            => ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), TimeSpan.FromHours(24)));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var newToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), TimeSpan.FromHours(24));

        Assert.Throws<DomainException>(() =>
        {
            newToken.Verify("invalid-token");
        });
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var value = Guid.NewGuid().ToString();
        var expiredConfirmationToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), TimeSpan.FromHours(-1));
        
        Assert.Throws<DomainException>(() =>
        {
            expiredConfirmationToken.Verify(value);
        });
    }
    
    [Fact]
    public void Compare()
    {
        var secureToken = ConfirmationTokenFixtures.GetSecureTokenizerStub().Generate();
        var expires = DateTime.UtcNow.AddHours(24);

        var token1 = new ConfirmationToken(secureToken, expires);
        var token2 = new ConfirmationToken(secureToken, expires);
        
        Assert.Equal(token1, token2);
    }
}