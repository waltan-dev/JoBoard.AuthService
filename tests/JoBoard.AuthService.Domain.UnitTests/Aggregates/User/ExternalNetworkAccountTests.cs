using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class ExternalNetworkAccountTests
{
    [Fact]
    public void CreateValid()
    {
        var externalUserId = "externalUserId";
        var externalNetwork = ExternalNetwork.Google;
        
        var newExternalNetworkAccount = new ExternalNetworkAccount(UserTestsHelper.DefaultUserId, externalUserId, externalNetwork);
        
        Assert.Equal(externalUserId, newExternalNetworkAccount.ExternalUserId);
        Assert.Equal(externalNetwork, newExternalNetworkAccount.Network);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new ExternalNetworkAccount(UserTestsHelper.DefaultUserId, " ", ExternalNetwork.Google);
        });
    }
}