using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ConfirmationTokenTests
{
    [Fact]
    public void GenerateValid()
    {
        var newToken = ConfirmationToken.Generate(expiresInHours: 1);
        
        Assert.NotEqual(Guid.Empty, Guid.Parse(newToken.Value));
        Assert.True(DateTime.UtcNow.AddMinutes(59) < newToken.Expiration);
        Assert.True(DateTime.UtcNow.AddMinutes(61) > newToken.Expiration);
    }
    
    [Fact]
    public void GenerateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = ConfirmationToken.Generate(expiresInHours: 0);
        });
    }
    
    [Fact]
    public void Validate()
    {
        var newToken = ConfirmationToken.Generate(expiresInHours: 1);
        var value = newToken.Value;

        var isValid = newToken.IsValid(value);
        
        Assert.True(isValid);
    }
    
    [Fact]
    public void ValidateInvalid()
    {
        var newToken = ConfirmationToken.Generate(expiresInHours: 1);
            
        var isValid = newToken.IsValid("invalid-token");
        
        Assert.False(isValid);
    }
    
    [Fact]
    public void ValidateExpired()
    {
        var newToken = ConfirmationToken.Generate(expiresInHours: -1);
        var value = newToken.Value;
            
        var isValid = newToken.IsValid(value);
        
        Assert.False(isValid);
    }
}