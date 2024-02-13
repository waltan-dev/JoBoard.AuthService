using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Integration.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser = BuildExistingActiveUser();
    
    public static IEnumerable<User> List => new List<User>()
    {
        ExistingActiveUser
    };
    
    private static User BuildExistingActiveUser()
    {
        // don't use TestsRegistry here because may be circular dependency

        // TODO clean code
        
        var userEmailUniquenessChecker = new Mock<IUserEmailUniquenessChecker>();
        userEmailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<Email>()))
            .Returns(() => true);
        
        var password = IntegrationTestsRegistry.UserPasswordBuilder.Create(PasswordFixtures.DefaultPassword);
        var user = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
            fullName: new FullName("Test", "Hirer"),
            email: new Email("ExistingActiveUser@gmail.com"),
            role: UserRole.Hirer,
            password: password,
            userEmailUniquenessChecker.Object);
        var confirmationToken = IntegrationTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user.RequestEmailConfirmation(confirmationToken, IntegrationTestsRegistry.CurrentDateTime);
        user.ConfirmEmail(user.EmailConfirmToken!.Value, IntegrationTestsRegistry.CurrentDateTime);
        return user;
    }
}