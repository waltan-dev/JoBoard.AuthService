using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class AttachExternalAccountTests
{
    [Fact]
    public void AttachExternalAccount()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccount("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Single(user.ExternalAccounts);
        Assert.Equal(externalAccount, user.ExternalAccounts.First());
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var user = new UserBuilder().WithActiveStatus().Build();

        var externalAccount = new ExternalAccount("externalUserId", ExternalAccountProvider.Google);
        user.AttachExternalAccount(externalAccount);
        user.AttachExternalAccount(externalAccount);
        
        Assert.Single(user.ExternalAccounts);
        Assert.Equal(externalAccount, user.ExternalAccounts.First());
    }
}