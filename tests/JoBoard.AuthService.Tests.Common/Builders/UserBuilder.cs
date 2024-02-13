using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserBuilder
{
    private bool _withGoogleAccountOption = false;
    private bool _withActiveStatusOption = false;
    private bool _withInactiveStatusOption = false;
    
    public User Build()
    {
        User user;
        if (_withGoogleAccountOption)
            user = User.RegisterByGoogleAccount(userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: new Email("ivan@gmail.com"),
                role: UserRole.Hirer,
                googleUserId: GoogleFixtures.UserProfileForNewUser.Id);
        else
        {
            var passwordHash = new PasswordBuilder().Create(PasswordFixtures.DefaultPassword);
            user = User.RegisterByEmailAndPassword(
                userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: new Email("ivan@gmail.com"),
                role: UserRole.Worker,
                passwordHash: passwordHash);
            //user.RequestEmailConfirmation(emailConfirmToken);
        }

        if (_withActiveStatusOption && user.Status.Equals(UserStatus.Active) == false)
        {
            var emailConfirmationToken = ConfirmationTokenFixtures.CreateNew();
            user.RequestEmailConfirmation(emailConfirmationToken);
            user.ConfirmEmail(emailConfirmationToken.Value);
        }
        
        if (_withInactiveStatusOption)
            user.Block();
        
        return user;
    }
    
    public UserBuilder WithActiveStatus()
    {
        if(_withInactiveStatusOption)
            throw new ArgumentException();
        
        _withActiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithInactiveStatus()
    {
        if (_withActiveStatusOption)
            throw new ArgumentException();
        
        _withInactiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithGoogleAccount()
    {
        _withGoogleAccountOption = true;
        return this;
    }
}