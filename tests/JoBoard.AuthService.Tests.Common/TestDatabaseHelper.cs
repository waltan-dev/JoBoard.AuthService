using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common;

public static class TestDatabaseHelper
{
    public static async Task InitializeAsync(AuthDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();
        await AddUserFixturesAsync(dbContext);
    }
    
    public static async Task ClearAsync(DbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"public\".\"Users\" CASCADE ;");
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"public\".\"ExternalAccounts\" CASCADE;");
    }

    private static async Task AddUserFixturesAsync(AuthDbContext dbContext)
    {
        var existingUsers = new List<User>
        {
            DbUserFixtures.ExistingActiveUser,
            DbUserFixtures.ExistingUserWithoutConfirmedEmail,
            DbUserFixtures.ExistingUserRegisteredByGoogleAccount,
            DbUserFixtures.ExistingUserWithExpiredToken
        };
        
        dbContext.Users.AddRange(existingUsers);
        await dbContext.SaveChangesAsync();
    }
    
    public static AuthDbContext CreateSqliteDbContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var dbContextOptions = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite(connection, builder => builder.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)).Options;
        return new AuthDbContext(dbContextOptions);
    }
}