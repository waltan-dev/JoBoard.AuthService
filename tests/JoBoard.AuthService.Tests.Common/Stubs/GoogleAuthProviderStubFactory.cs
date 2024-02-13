using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public static class GoogleAuthProviderStubFactory
{
    public static IGoogleAuthProvider Create()
    {
        var googleAuthProvider = new Mock<IGoogleAuthProvider>();
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(GoogleFixtures.IdTokenForExistingUser))
            .Returns(() => Task.FromResult(GoogleFixtures.UserProfileForExistingUser)!);
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(GoogleFixtures.IdTokenForNewUser))
            .Returns(() => Task.FromResult(GoogleFixtures.UserProfileForNewUser)!);
        
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(GoogleFixtures.IdTokenForNewUserWithExistingEmail))
            .Returns(() => Task.FromResult(GoogleFixtures.UserProfileForNewUserWithExistingEmail)!);

        return googleAuthProvider.Object;
    }
}