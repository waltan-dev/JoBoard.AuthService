using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Common;

public class UserBuilder
{
    private bool _withGoogleAccountOption = false;
    private bool _withActiveStatusOption = false;
    private bool _withAdminRoleOption = false;
    private bool _withExpiredRegisterTokenOption = false;
    
    public User Build()
    {
        User user;
        UserRole userRole = _withAdminRoleOption 
            ? UserRole.Admin 
            : UserRole.Hirer;
        var registerConfirmToken = _withExpiredRegisterTokenOption
            ? ConfirmationTokenFixtures.CreateExpired()
            : ConfirmationTokenFixtures.CreateNew();
        
        if (_withGoogleAccountOption)
            user = new User(
                userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: new Email("ivan@gmail.com"),
                role: userRole,
                externalAccount: new ExternalAccount(GoogleFixtures.UserProfileForNewUser.Id, ExternalAccountProvider.Google));
        else
            user = new User(
                userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: new Email("ivan@gmail.com"),
                role: userRole,
                passwordHash: PasswordFixtures.DefaultPasswordHash,
                registerConfirmToken: registerConfirmToken);
        
        if(_withActiveStatusOption && user.Status.Equals(UserStatus.Active) == false)
            user.ConfirmEmail(registerConfirmToken.Value);
        
        return user;
    }
    
    public UserBuilder WithActiveStatus()
    {
        _withActiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithGoogleAccount()
    {
        _withGoogleAccountOption = true;
        return this;
    }

    public UserBuilder WithAdminRole()
    {
        _withAdminRoleOption = true;
        return this;
    }
    
    public UserBuilder WithExpiredRegisterToken()
    {
        _withExpiredRegisterTokenOption = true;
        return this;
    }
}