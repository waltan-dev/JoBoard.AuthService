using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class ChangeEmailTests
{
    private static Email CreateNewEmail() => new("newEmail@gmail.com");
    
    [Fact]
    public void RequestChangeEmail()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.RequestEmailChange(newEmail, confirmationToken);
        
        var oldEmail = UserFixture.DefaultEmail;
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.NewEmailConfirmationToken);
    }
    
    [Fact]
    public void RequestChangeEmailIfUserIsNotActive()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var user = new UserBuilder().Build();
        var newEmail = CreateNewEmail();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestChangeEmailWithOldValue()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var oldEmail = UserFixture.DefaultEmail;
        var user = new UserBuilder().WithActiveStatus().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(oldEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestChangeEmailTwice()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var user = new UserBuilder().WithActiveStatus().Build();
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }
    
    [Fact]
    public void RequestChangeEmailTwiceAfterExpiration()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var oldEmail = UserFixture.DefaultEmail;
        var user = new UserBuilder().WithActiveStatus().Build();
        var newEmail = CreateNewEmail();
        user.RequestEmailChange(newEmail, UserFixture.CreateExpiredConfirmationToken());
        
        user.RequestEmailChange(newEmail, confirmationToken);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.NewEmailConfirmationToken);
    }
    
    [Fact]
    public void RequestChangeEmailWithInactiveStatus()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
        });
    }

    [Fact]
    public void ChangeEmail()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailChange(newEmail, confirmationToken);
        
        user.ChangeEmail(confirmationToken.Value);
        
        Assert.Equal(newEmail, user.Email);
        Assert.True(user.EmailConfirmed);
        Assert.Null(user.NewEmail);
        Assert.Null(user.NewEmailConfirmationToken);
    }
    
    [Fact]
    public void ChangeEmailWithInvalidToken()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var oldEmail = UserFixture.DefaultEmail;
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailChange(newEmail, confirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.ChangeEmail("invalid-token");
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ChangeEmailWithExpiredToken()
    {
        var expiredConfirmationToken = UserFixture.CreateExpiredConfirmationToken();
        var oldEmail = UserFixture.DefaultEmail;
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().WithActiveStatus().Build();
        user.RequestEmailChange(newEmail, expiredConfirmationToken);

        Assert.Throws<DomainException>(() =>
        {
            user.ChangeEmail(expiredConfirmationToken.Value);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ChangeEmailWithoutRequest()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var oldEmail = UserFixture.DefaultEmail;
        var user = new UserBuilder().WithActiveStatus().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.ChangeEmail(confirmationToken.Value);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ChangeEmailWithInactiveStatus()
    {
        var confirmationToken = UserFixture.CreateNewConfirmationToken();
        var newEmail = CreateNewEmail();
        var user = new UserBuilder().Build();
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken);
            user.ChangeEmail(confirmationToken.Value);
        });
    }
}