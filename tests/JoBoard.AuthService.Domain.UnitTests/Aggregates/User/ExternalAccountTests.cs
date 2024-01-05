using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ExternalAccountTests
{
    [Fact]
    public void CreateValid()
    {
        var externalUserId = "externalUserId";
        var provider = ExternalAccountProvider.Google;
        
        var newExternalAccount = new ExternalAccount(externalUserId, provider);
        
        Assert.Equal(externalUserId, newExternalAccount.ExternalUserId);
        Assert.Equal(provider, newExternalAccount.Provider);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new ExternalAccount(" ", ExternalAccountProvider.Google);
        });
    }
}