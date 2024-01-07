using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ConfirmationTokenTests
{
    [Fact]
    public void CreateValid()
    {
        var newToken = new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(1));
        
        Assert.NotEqual(Guid.Empty, Guid.Parse(newToken.Value));
        Assert.True(DateTime.UtcNow.AddMinutes(59) < newToken.Expiration);
        Assert.True(DateTime.UtcNow.AddMinutes(61) > newToken.Expiration);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new ConfirmationToken(" ", DateTime.UtcNow);
        });
    }
    
    [Fact]
    public void VerifyValid()
    {
        var value = Guid.NewGuid().ToString();
        var newToken = new ConfirmationToken(value, DateTime.UtcNow.AddHours(1));

        var isValid = newToken.Verify(value);
        
        Assert.True(isValid);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var value = Guid.NewGuid().ToString();
        var newToken = new ConfirmationToken(value, DateTime.UtcNow.AddHours(1));
            
        var isValid = newToken.Verify("invalid-token");
        
        Assert.False(isValid);
    }
    
    [Fact]
    public void VerifyExpired()
    {
        var value = Guid.NewGuid().ToString();
        var expiredConfirmationToken = UserFixture.CreateExpiredConfirmationToken();
        
        var isValid = expiredConfirmationToken.Verify(value);
        
        Assert.False(isValid);
    }
}