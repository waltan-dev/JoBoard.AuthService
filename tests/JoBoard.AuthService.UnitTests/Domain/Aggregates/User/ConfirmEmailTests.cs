using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ConfirmEmailTests
{
    [Fact]
    public void ConfirmEmailWithValidToken()
    {
        var userBuilder = new UserBuilder();
        var user = userBuilder.Build();

        user.ConfirmEmail(UserBuilder.DefaultConfirmationToken.Value);

        Assert.Equal(UserStatus.Active, user.Status);
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmailWithInvalidToken()
    {
        var user = new UserBuilder().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail("invalid-token");
        });
    }
    
    [Fact]
    public void ConfirmEmailWithExpiredToken()
    {
        var user = new UserBuilder().WithExpiredRegisterToken().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmail(user.RegisterConfirmToken!.Value);
        });
    }
}