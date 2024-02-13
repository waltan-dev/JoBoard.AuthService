using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common.Stubs;

public static class UserRepositoryStubFactory
{
    public static IUserRepository Create()
    {
        var existingUsers = new List<User>
        {
            DbUserFixtures.ExistingActiveUser,
            DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value,
            DbUserFixtures.ExistingUserWithExpiredToken.Value,
            DbUserFixtures.ExistingUserRegisteredByGoogleAccount.Value
        };
        var existingExternalAccounts = new List<ExternalAccount>
        {
            DbUserFixtures.ExistingUserRegisteredByGoogleAccount.Value.ExternalAccounts.First()
        };

        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var dbContextOptions = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite(connection, builder => builder.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)).Options;
        var dbContextToMock = new AuthDbContext(dbContextOptions);
        dbContextToMock.Database.EnsureCreated();
        
        dbContextToMock.Set<User>().AddRange(existingUsers);
        dbContextToMock.Set<ExternalAccount>().AddRange(existingExternalAccounts);
        dbContextToMock.SaveChanges();
        
        return new EfUserRepository(dbContextToMock, new EfUnitOfWork(dbContextToMock));
    }
}