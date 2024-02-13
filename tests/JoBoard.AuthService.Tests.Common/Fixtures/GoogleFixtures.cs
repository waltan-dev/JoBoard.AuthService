using JoBoard.AuthService.Application.Common.Services;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

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
        Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Email.Value
    };

    public static IGoogleAuthProvider GetGoogleAuthProviderStub()
    {
        var googleAuthProvider = new Mock<IGoogleAuthProvider>();
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(IdTokenForExistingUser))
            .Returns(() => Task.FromResult(UserProfileForExistingUser)!);
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(IdTokenForNewUser))
            .Returns(() => Task.FromResult(UserProfileForNewUser)!);
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(IdTokenForNewUserWithExistingEmail))
            .Returns(() => Task.FromResult(UserProfileForNewUserWithExistingEmail)!);

        return googleAuthProvider.Object;
    }
}