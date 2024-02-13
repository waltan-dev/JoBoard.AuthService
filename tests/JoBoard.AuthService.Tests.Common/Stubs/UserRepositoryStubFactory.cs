using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common.Stubs;

internal static class UserRepositoryStubFactory
{
    public static IUserRepository Create()
    {
        var dbContextToMock = TestDatabaseHelper.CreateSqliteDbContext();
        TestDatabaseHelper.InitializeAsync(dbContextToMock).GetAwaiter().GetResult();
        return new EfUserRepository(dbContextToMock, new EfUnitOfWork(dbContextToMock));
    }
}