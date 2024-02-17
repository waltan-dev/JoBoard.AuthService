using JoBoard.AuthService.Domain.Aggregates.UserAggregate;

namespace JoBoard.AuthService.Tests.Unit.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser = BuildExistingActiveUser();
    public static readonly User ExistingUserWithoutConfirmedEmail = BuildExistingUserWithoutConfirmedEmail();
    private static readonly User ExistingUserRegisteredByGoogleAccount = BuildExistingUserRegisteredByGoogleAccount();

    public static IEnumerable<User> List => new List<User>()
    {
        ExistingActiveUser,
        ExistingUserWithoutConfirmedEmail,
        ExistingUserRegisteredByGoogleAccount
    };
    
    private static User BuildExistingActiveUser()
    {
        return UnitTestsRegistry.UserBuilder
            .WithEmail("ExistingActiveUser@gmail.com")
            .WithActiveStatus()
            .Build();
    }
    
    private static User BuildExistingUserWithoutConfirmedEmail()
    {
        var user = UnitTestsRegistry.UserBuilder
            .WithEmail("ExistingUserRegisteredByEmail@gmail.com")
            .Build();
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        return user;
    }
    
    private static User BuildExistingUserRegisteredByGoogleAccount()
    {
        return UnitTestsRegistry.UserBuilder
            .WithEmail(GoogleFixtures.UserProfileForExistingUser.Email)
            .WithGoogleAccount(GoogleFixtures.UserProfileForExistingUser.Id)
            .Build();
    }
}