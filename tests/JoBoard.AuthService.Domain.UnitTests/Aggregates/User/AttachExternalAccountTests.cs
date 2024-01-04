using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class AttachExternalAccountTests
{
    [Fact]
    public void AttachExternalAccount()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google);
        user.AttachNetwork(externalAccount);
        
        Assert.Equal(1, user.ExternalNetworkAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalNetworkAccounts.First());
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google);
        user.AttachNetwork(externalAccount);
        user.AttachNetwork(externalAccount);
        
        Assert.Equal(1, user.ExternalNetworkAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalNetworkAccounts.First());
    }
    
    [Fact]
    public void AttachExternalAccountWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        var externalAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google);

        Assert.Throws<DomainException>(() =>
        {
            user.AttachNetwork(externalAccount);
        });
    }
}