using JoBoard.AuthService.Domain.Aggregates.UserAggregate;

namespace JoBoard.AuthService.Tests.Functional.Fixtures;

public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser01 = BuildExistingActiveUser("ExistingActiveUser01@gmail.com");
    public static readonly User ExistingActiveUser02 = BuildExistingActiveUser("ExistingActiveUser02@gmail.com");
    public static readonly User ExistingUserWithoutConfirmedEmail01 = BuildExistingUserWithoutConfirmedEmail("ExistingUserWithoutConfirmedEmail01@gmail.com");
    public static readonly User ExistingUserWithoutConfirmedEmail02 = BuildExistingUserWithoutConfirmedEmail("ExistingUserWithoutConfirmedEmail02@gmail.com");
    private static readonly User ExistingUserRegisteredByGoogleAccount = BuildExistingUserRegisteredByGoogleAccount();
    
    public static IEnumerable<User> List()
    {
        yield return ExistingActiveUser01;
        yield return ExistingActiveUser02;
        yield return ExistingUserWithoutConfirmedEmail01;
        yield return ExistingUserWithoutConfirmedEmail02;
        yield return ExistingUserRegisteredByGoogleAccount;
    }
    
    private static User BuildExistingActiveUser(string email)
    {
        return FunctionalTestsRegistry.UserBuilder
            .WithEmail(email)
            .WithActiveStatus()
            .Build();
    }
    
    private static User BuildExistingUserWithoutConfirmedEmail(string email)
    {
        var user = FunctionalTestsRegistry.UserBuilder
            .WithEmail(email)
            .Build();
        var token = FunctionalTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(token, FunctionalTestsRegistry.CurrentDateTime);
        return user;
    }
    
    private static User BuildExistingUserRegisteredByGoogleAccount()
    {
        return FunctionalTestsRegistry.UserBuilder
            .WithEmail(GoogleFixtures.UserProfileForExistingUser.Email)
            .WithGoogleAccount(GoogleFixtures.UserProfileForExistingUser.Id)
            .Build();
    }
}