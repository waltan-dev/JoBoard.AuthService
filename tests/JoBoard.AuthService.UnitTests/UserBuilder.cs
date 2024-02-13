using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

namespace JoBoard.AuthService.UnitTests;

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
            : UserFixture.DefaultUserRole;
        var registerConfirmToken = _withExpiredRegisterTokenOption
            ? UserFixture.CreateExpiredConfirmationToken()
            : UserFixture.DefaultConfirmationToken;
        if (_withGoogleAccountOption == false)
            user = new User(
                userId: UserFixture.DefaultUserId,
                fullName: UserFixture.DefaultFullName,
                email: UserFixture.DefaultEmail,
                role: userRole,
                passwordHash: UserFixture.DefaultPasswordHash,
                registerConfirmToken: registerConfirmToken);
        else
            user = new User(
                userId: UserFixture.DefaultUserId,
                fullName: UserFixture.DefaultFullName,
                email: UserFixture.DefaultEmail,
                role: userRole,
                externalAccount: UserFixture.DefaultGoogleAccount);
        
        if(_withActiveStatusOption)
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