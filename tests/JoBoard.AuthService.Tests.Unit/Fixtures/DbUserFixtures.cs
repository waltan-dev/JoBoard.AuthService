using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Unit.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser = BuildExistingActiveUser();
    public static readonly User ExistingUserWithoutConfirmedEmail = BuildExistingUserWithoutConfirmedEmail();
    private static readonly User ExistingUserRegisteredByGoogleAccount = BuildExistingUserRegisteredByGoogleAccount();

    public static IEnumerable<User> List => new List<User>()
    {
        ExistingActiveUser,
        ExistingUserWithoutConfirmedEmail,
        ExistingUserRegisteredByGoogleAccount
    };
    
    private static User BuildExistingActiveUser()
    {
        // don't use TestsRegistry here because may be circular dependency

        // TODO clean code
        
        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingActiveUser@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
        user.ConfirmEmail(user.EmailConfirmToken!.Value, UnitTestsRegistry.CurrentDateTime);
        return user;
    }
    
    private static User BuildExistingUserWithoutConfirmedEmail()
    {
        // don't use TestsRegistry here because may be circular dependency
        
        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = UnitTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(
            userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var confirmationToken = UnitTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(confirmationToken, UnitTestsRegistry.CurrentDateTime);
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
}