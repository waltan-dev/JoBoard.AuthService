using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ConfirmationTokenTest
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
}