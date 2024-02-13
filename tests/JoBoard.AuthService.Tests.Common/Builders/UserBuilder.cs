using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserBuilder
{
    private readonly ConfirmationTokenBuilder _confirmationTokenBuilder = new();
    
    private bool WithGoogleAccountOption { get; set; } = false;
    private bool WithActiveStatusOption { get; set; } = false;
    private bool WithInactiveStatusOption { get; set; } = false;
    
    public User Build()
    {
        User user;
        if (WithGoogleAccountOption)
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

        if (WithActiveStatusOption && user.Status.Equals(UserStatus.Active) == false)
        {
            var emailConfirmationToken = _confirmationTokenBuilder.BuildActive();
            user.RequestEmailConfirmation(emailConfirmationToken);
            user.ConfirmEmail(emailConfirmationToken.Value);
        }
        
        if (WithInactiveStatusOption)
            user.Block();
        
        return user;
    }
    
    public UserBuilder WithActiveStatus()
    {
        if(WithInactiveStatusOption)
            throw new ArgumentException();
        
        WithActiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithInactiveStatus()
    {
        if (WithActiveStatusOption)
            throw new ArgumentException();
        
        WithInactiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithGoogleAccount()
    {
        WithGoogleAccountOption = true;
        return this;
    }
}