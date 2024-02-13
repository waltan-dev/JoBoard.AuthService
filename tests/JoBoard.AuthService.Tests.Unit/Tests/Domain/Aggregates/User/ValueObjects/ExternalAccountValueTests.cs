using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User.ValueObjects;

public class ExternalAccountValueTests
{
    [Fact]
    public void CreateValid()
    {
        var externalUserId = "externalUserId";
        var provider = ExternalAccountProvider.Google;
        
        var newExternalAccount = new ExternalAccountValue(externalUserId, provider);
        
        Assert.Equal(externalUserId, newExternalAccount.ExternalUserId);
        Assert.Equal(provider, newExternalAccount.Provider);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new ExternalAccountValue(" ", ExternalAccountProvider.Google);
        });
    }
}