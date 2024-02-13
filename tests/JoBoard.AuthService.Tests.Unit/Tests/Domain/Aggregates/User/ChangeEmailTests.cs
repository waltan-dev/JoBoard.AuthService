using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User;

public class ChangeEmailTests
{
    private static Email CreateNewEmail() => new("newEmail@gmail.com");
    
    [Fact]
    public void RequestEmailChange()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = CreateNewEmail();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserRequestedEmailChangeDomainEvent);
    }
    
    [Fact]
    public void RequestEmailChangeWithoutConfirmedEmail()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.Build();
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailChangeWithInactiveStatus()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailChangeWithOldValue()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = DbUserFixtures.ExistingActiveUser;
        var oldEmail = user.Email;
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(oldEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailChangeTwiceBeforeExpiration()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void RequestEmailChangeTwiceAfterExpiration()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        
        confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.FutureDateTime);
        
        Assert.Equal(oldEmail, user.Email);
        Assert.Equal(newEmail, user.NewEmail);
        Assert.Equal(confirmationToken, user.ChangeEmailConfirmToken);
        Assert.Contains(user.DomainEvents, ev => ev is UserRequestedEmailChangeDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailChange()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = CreateNewEmail();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
        
        user.ConfirmEmailChange(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        
        Assert.Equal(newEmail, user.Email);
        Assert.True(user.EmailConfirmed);
        Assert.Null(user.NewEmail);
        Assert.Null(user.ChangeEmailConfirmToken);
        Assert.Single(user.DomainEvents, ev => ev is UserChangedEmailDomainEvent);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInvalidToken()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange("invalid-token", UnitTestsRegistry.CurrentDateTime);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithExpiredToken()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;
        var newEmail = CreateNewEmail();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange(confirmationToken.Value, UnitTestsRegistry.FutureDateTime);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithoutRequest()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var user = UnitTestsRegistry.UserBuilder.WithActiveStatus().Build();
        var oldEmail = user.Email;

        Assert.Throws<DomainException>(() =>
        {
            user.ConfirmEmailChange(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        });
        Assert.Equal(oldEmail, user.Email);
    }
    
    [Fact]
    public void ConfirmEmailChangeWithoutConfirmedEmail()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = CreateNewEmail();
        var user = UnitTestsRegistry.UserBuilder.Build();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
            user.ConfirmEmailChange(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        });
    }
    
    [Fact]
    public void ConfirmEmailChangeWithInactiveStatus()
    {
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = CreateNewEmail();
        var user = UnitTestsRegistry.UserBuilder.WithInactiveStatus().Build();
        var userEmailUniquenessChecker = UnitTestsRegistry.UserEmailUniquenessChecker;
        
        Assert.Throws<DomainException>(() =>
        {
            user.RequestEmailChange(newEmail, confirmationToken, userEmailUniquenessChecker, UnitTestsRegistry.CurrentDateTime);
            user.ConfirmEmailChange(confirmationToken.Value, UnitTestsRegistry.CurrentDateTime);
        });
    }
}