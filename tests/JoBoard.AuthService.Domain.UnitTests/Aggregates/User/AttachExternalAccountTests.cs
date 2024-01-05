using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class AttachExternalAccountTests
{
    [Fact]
    public void AttachExternalAccount()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccount(UserTestsHelper.DefaultUserId,"externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Equal(1, user.ExternalAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalAccounts.First());
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccount(UserTestsHelper.DefaultUserId,"externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Equal(1, user.ExternalAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalAccounts.First());
    }
    
    [Fact]
    public void AttachExternalAccountWithInactiveStatus()
    {
        var user = new UserBuilder().Build();
        var externalAccount = new ExternalAccount(UserTestsHelper.DefaultUserId,"externalUserId", ExternalAccountProvider.Google);

        Assert.Throws<DomainException>(() =>
        {
            user.AttachExternalAccount(externalAccount);
        });
    }
}