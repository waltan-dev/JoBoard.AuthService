using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests;

public class UserBuilder
{
    private bool _withExternalAccountOption = false;
    private bool _withActiveStatusOption = false;
    private bool _withAdminRoleOption = false;
    
    public User Build()
    {
        User user;
        UserRole userRole = _withAdminRoleOption ? UserRole.Admin : UserTestsHelper.DefaultUserRole;
        if(_withExternalAccountOption == false)
            user = new User(
                userId: UserTestsHelper.DefaultUserId,
                fullName: UserTestsHelper.DefaultFullName,
                email: UserTestsHelper.DefaultEmail,
                role: userRole,
                passwordHash: UserTestsHelper.DefaultPasswordHash,
                registerConfirmToken: UserTestsHelper.DefaultConfirmationToken);
        else
            user = new User(
                userId: UserTestsHelper.DefaultUserId,
                fullName: UserTestsHelper.DefaultFullName,
                email: UserTestsHelper.DefaultEmail,
                role: userRole,
                externalNetworkAccount: UserTestsHelper.DefaultExternalNetworkAccount,
                registerConfirmToken: UserTestsHelper.DefaultConfirmationToken);
        
        if(_withActiveStatusOption)
            user.ConfirmEmail(UserTestsHelper.DefaultConfirmationToken.Value);

        return user;
    }
    
    public UserBuilder WithActiveStatus()
    {
        _withActiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithExternalAccount()
    {
        _withExternalAccountOption = true;
        return this;
    }

    public UserBuilder WithAdminRole()
    {
        _withAdminRoleOption = true;
        return this;
    }
}