using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    private static object _lock = new();
    private static User? _existingActiveUser;
    public static User ExistingActiveUser 
    {
        get
        {
            lock (_lock)
            {
                if (_existingActiveUser != null)
                    return _existingActiveUser;
                
                var user = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
                    fullName: new FullName("Test", "Hirer"),
                    email: new Email("ExistingActiveUser@gmail.com"),
                    role: UserRole.Hirer,
                    passwordHash: PasswordFixtures.CreateDefault());
                
                user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateNew());
                user.ConfirmEmail(user.EmailConfirmToken!.Value);
                _existingActiveUser = user;
            
                return _existingActiveUser; 
            }
        }
    }

    public static readonly Lazy<User> ExistingUserWithoutConfirmedEmail = new(() =>
    {
        var user = User.RegisterByEmailAndPassword(
            userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
            role: UserRole.Hirer,
            passwordHash: PasswordFixtures.CreateDefault());
        user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateNew());
        return user;
    });
    
    public static readonly Lazy<User> ExistingUserRegisteredByGoogleAccount = new(() =>
    {
        return User.RegisterByGoogleAccount(
            userId: UserId.Generate(),
            fullName: new FullName(GoogleFixtures.UserProfileForExistingUser.FirstName,
                GoogleFixtures.UserProfileForExistingUser.LastName),
            email: new Email(GoogleFixtures.UserProfileForExistingUser.Email),
            role: UserRole.Worker,
            googleUserId: GoogleFixtures.UserProfileForExistingUser.Id);
    });
    
    public static readonly Lazy<User> ExistingUserWithExpiredToken = new(() =>
    {
        var user = User.RegisterByEmailAndPassword(
            userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingUserWithExpiredToken@gmail.com"),
            role: UserRole.Hirer,
            passwordHash: PasswordFixtures.CreateDefault());
        user.RequestEmailConfirmation(ConfirmationTokenFixtures.CreateExpired());
        return user;
    });
}