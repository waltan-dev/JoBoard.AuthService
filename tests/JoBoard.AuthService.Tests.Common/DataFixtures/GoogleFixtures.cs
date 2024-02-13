using JoBoard.AuthService.Application.Services;

namespace JoBoard.AuthService.Tests.Common.DataFixtures;

public static class GoogleFixtures
{
    public static readonly string IdTokenForExistingUser = "IdTokenForExistingUser";
    public static readonly GoogleUserProfile UserProfileForExistingUser = new()
    {
        Id = "existing_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "emailForExistingUser@gmail.com"
    };
    
    public static readonly string IdTokenForNewUser = "IdTokenForNewUser";
    public static readonly GoogleUserProfile UserProfileForNewUser = new()
    {
        Id = "new_user_id",
        FirstName = "Test",
        LastName = "test",
        Email = "emailForNewUser@gmail.com"
    };
    
    public static readonly string IdTokenForNewUserWithExistingEmail = "IdTokenForNewUserWithExistingEmail";
    public static readonly GoogleUserProfile UserProfileForNewUserWithExistingEmail = new()
    {
        Id = "NewUserWithExistingEmailId",
        FirstName = "Test",
        LastName = "test",
        Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Email.Value
    };
}