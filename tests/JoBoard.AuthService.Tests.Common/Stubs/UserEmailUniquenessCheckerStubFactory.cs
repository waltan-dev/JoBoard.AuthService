using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public class UserEmailUniquenessCheckerStubFactory
{
    public IUserEmailUniquenessChecker Create(IEnumerable<User> existingUsers)
    {
        var stub = new Mock<IUserEmailUniquenessChecker>();
        var existingUsersEmails = existingUsers.Select(x => x.Email);
        stub.Setup(x => x.IsUnique(It.IsIn(existingUsersEmails)))
            .Returns(() => false);
        stub.Setup(x => x.IsUnique(It.IsNotIn(existingUsersEmails)))
            .Returns(() => true);

        return stub.Object;
    }
}