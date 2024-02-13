using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;

namespace JoBoard.AuthService.Tests.Unit.Stubs;

public static class UserRepositoryStubFactory
{
    public static IUserRepository Create(IEnumerable<User> users)
    {
        var dbContextToMock = TestDatabaseHelper.CreateSqliteDbContext();
        TestDatabaseHelper.InitializeAsync(dbContextToMock, users).GetAwaiter().GetResult();
        return new EfUserRepository(dbContextToMock, new EfUnitOfWork(dbContextToMock));
    }
}