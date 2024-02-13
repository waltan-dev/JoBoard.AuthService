using Google.Apis.Auth;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Infrastructure.Auth.Configs;

namespace JoBoard.AuthService.Infrastructure.Auth.Services;

public class GoogleAuthProvider : IGoogleAuthProvider
{
    private readonly GoogleAuthConfig _googleAuthConfig;

    public GoogleAuthProvider(GoogleAuthConfig googleAuthConfig)
    {
        _googleAuthConfig = googleAuthConfig;
    }
    
    public async Task<GoogleUserProfile?> VerifyIdTokenAsync(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            if (payload.Audience.Equals(_googleAuthConfig.ClientId) == false)
                return null;
            if (payload.Issuer.Equals("accounts.google.com") == false && payload.Issuer.Equals("https://accounts.google.com") == false)
                return null;
            if (payload.ExpirationTimeSeconds == null)            
                return null;
            
            var now = DateTime.Now.ToUniversalTime();
            var expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
            if (now > expiration)
                return null;

            return new GoogleUserProfile
            {
                Id = payload.Subject,
                Email = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName
            };
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}