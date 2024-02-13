using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using Moq;

namespace JoBoard.AuthService.Tests.Unit.Stubs;

public class ExternalAccountUniquenessCheckerStubFactory
{
    public IExternalAccountUniquenessChecker Create(IEnumerable<User> existingUsers)
    {
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