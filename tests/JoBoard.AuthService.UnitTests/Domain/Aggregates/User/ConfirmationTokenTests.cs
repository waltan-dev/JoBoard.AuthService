using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var newToken = ConfirmationToken.Generate();
        
        Assert.NotEqual(Guid.Empty, Guid.Parse(newToken.Value));
        Assert.True(newToken.Expiration > DateTime.UtcNow);
    }
    
    [Fact]
    public void VerifyValid()
    {
        var newToken = ConfirmationToken.Generate();

        var isValid = newToken.Verify(newToken.Value);
        
        Assert.True(isValid);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var newToken = ConfirmationToken.Generate();
            
        var isValid = newToken.Verify("invalid-token");
        
        Assert.False(isValid);
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var value = Guid.NewGuid().ToString();
        var expiredConfirmationToken = ConfirmationToken.Generate(-1);
        
        var isValid = expiredConfirmationToken.Verify(value);
        
        Assert.False(isValid);
    }
}