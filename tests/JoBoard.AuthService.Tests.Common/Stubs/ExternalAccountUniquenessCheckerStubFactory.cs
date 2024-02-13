using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

internal static class ExternalAccountUniquenessCheckerStubFactory
{
    public static IExternalAccountUniquenessChecker Create()
    {
        var existingUsers = new List<User>
        {
            DbUserFixtures.ExistingActiveUser,
            DbUserFixtures.ExistingUserWithoutConfirmedEmail,
            DbUserFixtures.ExistingUserRegisteredByGoogleAccount,
            DbUserFixtures.ExistingUserWithExpiredToken
        };
        
        var stub = new Mock<IExternalAccountUniquenessChecker>();
        var existingExternalAccounts = existingUsers
            .SelectMany(x => x.ExternalAccounts)
            .Select(x => x.Value);
        stub.Setup(x => x.IsUnique(It.IsIn(existingExternalAccounts)))
            .Returns(() => false);
        stub.Setup(x => x.IsUnique(It.IsNotIn(existingExternalAccounts)))
            .Returns(() => true);

        return stub.Object;
    }
}