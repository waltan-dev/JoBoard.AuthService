using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Builders;

public class UserBuilder
{
    private bool _withExternalAccountOption = false;
    private bool _withActiveStatusOption = false;
    
    public User Build()
    {
        User user;
        if(_withExternalAccountOption == false)
            user = new User(
                userId: UserTestsHelper.DefaultUserId,
                fullName: UserTestsHelper.DefaultFullName,
                email: UserTestsHelper.DefaultEmail,
                accountType: UserTestsHelper.DefaultAccountType,
                passwordHash: UserTestsHelper.DefaultPasswordHash,
                confirmationToken: UserTestsHelper.DefaultConfirmationToken);
        else
            user = new User(
                userId: UserTestsHelper.DefaultUserId,
                fullName: UserTestsHelper.DefaultFullName,
                email: UserTestsHelper.DefaultEmail,
                accountType: UserTestsHelper.DefaultAccountType,
                externalNetworkAccount: UserTestsHelper.DefaultExternalNetworkAccount,
                confirmationToken: UserTestsHelper.DefaultConfirmationToken);
        
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
}