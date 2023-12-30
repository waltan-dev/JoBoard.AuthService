using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common;
using JoBoard.AuthService.Domain.UnitTests.Builders;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class UserTests
{
    [Fact]
    public void CreateNewUserByEmailAndPassword()
    {
        var userBuilder = new UserBuilder();
        var newUser = userBuilder.Build();

        Assert.Equal(userBuilder.UserId, newUser.Id);
        Assert.Equal(userBuilder.FullName, newUser.FullName);
        Assert.Equal(userBuilder.Email, newUser.Email);
        Assert.Equal(userBuilder.AccountType, newUser.AccountType);
        Assert.Equal(userBuilder.PasswordHash, newUser.PasswordHash);
        Assert.Equal(userBuilder.ConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
    }
    
    [Fact]
    public void CreateNewUserByExternalNetworkAccount()
    {
        var userBuilder = new UserBuilder
        {
            ExternalNetworkAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google)
        };
        var newUser = userBuilder.Build();

        Assert.Equal(userBuilder.UserId, newUser.Id);
        Assert.Equal(userBuilder.FullName, newUser.FullName);
        Assert.Equal(userBuilder.Email, newUser.Email);
        Assert.Equal(userBuilder.AccountType, newUser.AccountType);
        Assert.Equal(userBuilder.ConfirmationToken, newUser.ConfirmationToken);
        Assert.Equal(UserStatus.Pending, newUser.Status);
        Assert.False(newUser.EmailConfirmed);
        Assert.NotEqual(default, newUser.RegisteredAt);
        Assert.Null(newUser.PasswordHash);
        Assert.Equal(userBuilder.ExternalNetworkAccount, newUser.ExternalNetworkAccounts.First());
    }

    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var userBuilder = new UserBuilder();
        var user = userBuilder.Build();

        user.ConfirmEmail(userBuilder.ConfirmationToken.Value);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var userBuilder = new UserBuilder { ConfirmationToken = ConfirmationToken.Generate(expiresInHours: -1) };
        var user = userBuilder.Build();

        Assert.Throws<DomainException>(() => { user.ConfirmEmail(userBuilder.ConfirmationToken.Value); });
    }
    
    [Fact]
    public void AttachExternalAccount()
    {
        var userBuilder = new UserBuilder { UserStatus = UserStatus.Active };
        var user = userBuilder.Build();

        var externalAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google);
        user.AttachNetwork(externalAccount);
        
        Assert.Equal(1, user.ExternalNetworkAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalNetworkAccounts.First());
    }
    
    [Fact]
    public void AttachSameExternalAccountTwice()
    {
        var userBuilder = new UserBuilder { UserStatus = UserStatus.Active };
        var user = userBuilder.Build();

        var externalAccount = new ExternalNetworkAccount("externalUserId", ExternalNetwork.Google);
        user.AttachNetwork(externalAccount);
        user.AttachNetwork(externalAccount);
        
        Assert.Equal(1, user.ExternalNetworkAccounts.Count);
        Assert.Equal(externalAccount, user.ExternalNetworkAccounts.First());
    }
}