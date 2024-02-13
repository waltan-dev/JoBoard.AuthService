using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Tests.Common;

public class UserBuilder
{
    private bool _withGoogleAccountOption = false;
    private bool _withActiveStatusOption = false;
    private bool _withAdminRoleOption = false;
    private bool _withExpiredRegisterTokenOption = false;
    
    public static readonly UserId DefaultUserId = UserId.Generate();
    public static readonly FullName DefaultFullName = new("Ivan", "Ivanov");
    public static readonly Email DefaultEmail = new("ivan@gmail.com");
    public static readonly UserRole DefaultUserRole = UserRole.Hirer;
    public static readonly string DefaultPasswordHash = "DefaultPasswordHash";
    public static readonly string DefaultPassword = "DefaultPassword";
    public static readonly ConfirmationToken DefaultConfirmationToken = ConfirmationToken.Generate();
    public static readonly ExternalAccount DefaultGoogleAccount =
        new("googleUserId", ExternalAccountProvider.Google);
    
    public User Build()
    {
        User user;
        UserRole userRole = _withAdminRoleOption 
            ? UserRole.Admin 
            : DefaultUserRole;
        var registerConfirmToken = _withExpiredRegisterTokenOption
            ? UserFixtures.CreateExpiredConfirmationToken()
            : DefaultConfirmationToken;
        if (_withGoogleAccountOption == false)
            user = new User(
                userId: DefaultUserId,
                fullName: DefaultFullName,
                email: DefaultEmail,
                role: userRole,
                passwordHash: DefaultPasswordHash,
                registerConfirmToken: registerConfirmToken);
        else
            user = new User(
                userId: DefaultUserId,
                fullName: DefaultFullName,
                email: DefaultEmail,
                role: userRole,
                externalAccount: DefaultGoogleAccount);
        
        if(_withActiveStatusOption && _withGoogleAccountOption == false)
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