using JoBoard.AuthService.Application.Common.Services;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class GoogleAuthProviderStubFactory
{
    public IGoogleAuthProvider Create(Dictionary<string, GoogleUserProfile> googleUserProfiles)
    {
        var googleAuthProvider = new Mock<IGoogleAuthProvider>();

        foreach (var (idToken, profile) in googleUserProfiles)
        {
            googleAuthProvider
                .Setup(x => x.VerifyIdTokenAsync(idToken))
                .ReturnsAsync(() => profile);
        }

        var idTokens = googleUserProfiles.Keys.ToArray();
        googleAuthProvider
            .Setup(x => x.VerifyIdTokenAsync(It.IsNotIn(idTokens)))
            .ReturnsAsync(() => null);
        
        return googleAuthProvider.Object;
    }
}