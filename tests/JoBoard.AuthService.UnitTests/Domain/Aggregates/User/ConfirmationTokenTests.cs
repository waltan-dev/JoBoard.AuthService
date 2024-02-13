using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var newToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), 24);
        
        Assert.NotEmpty(newToken.Value);
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void VerifyValid()
    {
        var exception = Record.Exception(() 
            => ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), 24));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var newToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), 24);

        Assert.Throws<DomainException>(() =>
        {
            newToken.Verify("invalid-token");
        });
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var value = Guid.NewGuid().ToString();
        var expiredConfirmationToken = ConfirmationToken.Create(ConfirmationTokenFixtures.GetSecureTokenizerStub(), -1);
        
        Assert.Throws<DomainException>(() =>
        {
            expiredConfirmationToken.Verify(value);
        });
    }
}