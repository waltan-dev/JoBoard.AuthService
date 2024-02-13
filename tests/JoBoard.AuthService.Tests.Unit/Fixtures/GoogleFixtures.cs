using JoBoard.AuthService.Application.Contracts;

namespace JoBoard.AuthService.Tests.Unit.Fixtures;

public static class GoogleFixtures
{
    public const string IdTokenForExistingUser = "IdTokenForExistingUser";
    public static readonly GoogleUserProfile UserProfileForExistingUser = new()
    {
        Id = "existing_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "emailForExistingUser@gmail.com"
    };

    public const string IdTokenForNewUser = "IdTokenForNewUser";
    private static readonly GoogleUserProfile UserProfileForNewUser = new()
    {
        Id = "new_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "emailForNewUser@gmail.com"
    };

    public const string IdTokenForNewUserWithExistingEmail = "IdTokenForNewUserWithExistingEmail";
    private static readonly GoogleUserProfile UserProfileForNewUserWithExistingEmail = new()
    {
        Id = "NewUserWithExistingEmailId",
        FirstName = "Test",
        LastName = "test",
        Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Email.Value
    };

    public static Dictionary<string, GoogleUserProfile> Dictionary => new()
    {
        { IdTokenForExistingUser, UserProfileForExistingUser },
        { IdTokenForNewUser, UserProfileForNewUser },
        { IdTokenForNewUserWithExistingEmail, UserProfileForNewUserWithExistingEmail },
    };
}