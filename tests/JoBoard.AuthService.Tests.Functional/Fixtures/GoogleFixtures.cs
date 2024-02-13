using JoBoard.AuthService.Application.Common.Services;

namespace JoBoard.AuthService.Tests.Functional.Fixtures;

public static class GoogleFixtures
{
    public const string IdTokenForExistingUser = "IdTokenForExistingUser";
    public static readonly GoogleUserProfile UserProfileForExistingUser = new()
    {
        Id = "existing_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "googleEmailForExistingUser@gmail.com"
    };

    public const string IdTokenForNewUser = "IdTokenForNewUser";
    public static readonly GoogleUserProfile UserProfileForNewUser = new()
    {
        Id = "new_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "googleEmailForNewUser@gmail.com"
    };

    public static Dictionary<string, GoogleUserProfile> Dictionary = new()
    {
        { IdTokenForExistingUser, UserProfileForExistingUser },
        { IdTokenForNewUser, UserProfileForNewUser }
    };
}