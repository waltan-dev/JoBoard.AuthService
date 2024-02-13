using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Common.Builders;
using Moq;

namespace JoBoard.AuthService.Tests.Common.DataFixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser = BuildExistingActiveUser();
    public static readonly User ExistingUserWithoutConfirmedEmail = BuildExistingUserWithoutConfirmedEmail();
    public static readonly User ExistingUserWithExpiredToken = BuildExistingUserWithExpiredToken();
    public static readonly User ExistingUserRegisteredByGoogleAccount = BuildExistingUserRegisteredByGoogleAccount();
    
    private static User BuildExistingActiveUser()
    {
        // don't use TestsRegistry here because may be circular dependency

        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = new UserPasswordBuilder().Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingActiveUser@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestEmailConfirmation(confirmationToken);
        user.ConfirmEmail(user.EmailConfirmToken!.Value);
        return user;
    }
    
    private static User BuildExistingUserWithoutConfirmedEmail()
    {
        // don't use TestsRegistry here because may be circular dependency
        
        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = new UserPasswordBuilder().Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(
            userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var confirmationToken = new ConfirmationTokenBuilder().BuildActive();
        user.RequestEmailConfirmation(confirmationToken);
        return user;
    }
    
    private static User BuildExistingUserRegisteredByGoogleAccount()
    {
        // don't use TestsRegistry here because may be circular dependency
        
        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var externalAccountUniquenessChecker = new Mock<IExternalAccountUniquenessChecker>();
        externalAccountUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<ExternalAccountValue>()))
            .Returns(() => true);
        
        return User.RegisterByGoogleAccount(
            userId: UserId.Generate(),
            fullName: new FullName(GoogleFixtures.UserProfileForExistingUser.FirstName,
                GoogleFixtures.UserProfileForExistingUser.LastName),
            email: new Email(GoogleFixtures.UserProfileForExistingUser.Email),
            role: UserRole.Worker,
            googleUserId: GoogleFixtures.UserProfileForExistingUser.Id,
            userEmailUniquenessChecker.Object,
            externalAccountUniquenessChecker.Object);
    }
    
    private static User BuildExistingUserWithExpiredToken()
    {
        // don't use TestsRegistry here because may be circular dependency

        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = new UserPasswordBuilder().Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(
            userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingUserWithExpiredToken@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var expiredToken = new ConfirmationTokenBuilder().BuildExpired();
        user.RequestEmailConfirmation(expiredToken);
        return user;
    }
}