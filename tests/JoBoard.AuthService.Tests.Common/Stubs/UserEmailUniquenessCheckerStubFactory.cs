using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

internal class UserEmailUniquenessCheckerStubFactory
{
    public static IUserEmailUniquenessChecker Create()
    {
        var existingUsers = new List<User>
        {
            DbUserFixtures.ExistingActiveUser,
            DbUserFixtures.ExistingUserWithoutConfirmedEmail,
            DbUserFixtures.ExistingUserRegisteredByGoogleAccount,
            DbUserFixtures.ExistingUserWithExpiredToken
        };
        
        var stub = new Mock<IUserEmailUniquenessChecker>();
        var existingUsersEmails = existingUsers.Select(x => x.Email);
        stub.Setup(x => x.IsUnique(It.IsIn(existingUsersEmails)))
            .Returns(() => false);
        stub.Setup(x => x.IsUnique(It.IsNotIn(existingUsersEmails)))
            .Returns(() => true);

        return stub.Object;
    }
}