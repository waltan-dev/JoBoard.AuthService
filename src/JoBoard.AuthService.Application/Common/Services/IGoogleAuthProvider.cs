namespace JoBoard.AuthService.Application.Common.Services;

public interface IGoogleAuthProvider
{
    Task<GoogleUserProfile?> VerifyIdTokenAsync(string idToken);
}

public class GoogleUserProfile
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}