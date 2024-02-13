using JoBoard.AuthService.Application.Services;
using Moq;

namespace JoBoard.AuthService.Tests.Common;

public static class GoogleFixture
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

    public static IGoogleAuthProvider GetGoogleAuthProvider()
    {
        var googleAuthProvider = new Mock<IGoogleAuthProvider>();
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(IdTokenForExistingUser))
            .Returns(() => Task.FromResult(UserProfileForExistingUser)!);
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(IdTokenForNewUser))
            .Returns(() => Task.FromResult(UserProfileForNewUser)!);

        return googleAuthProvider.Object;
    }
}