using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Tests.Integration.Fixtures;

namespace JoBoard.AuthService.Tests.Integration.Tests.Infrastructure.Data;

// single DatabaseFixture per class - shared data per class
public abstract class BaseRepositoryTest : IClassFixture<DatabaseFixture> 
{
    protected IUserRepository UserRepository { get; private set; }

    protected BaseRepositoryTest(DatabaseFixture databaseFixture)
    {
        UserRepository = IntegrationTestsRegistry.GetUserRepository(databaseFixture.ConnectionString);
    }
}