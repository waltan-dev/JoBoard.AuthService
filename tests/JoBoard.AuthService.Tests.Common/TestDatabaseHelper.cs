using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common;

public static class TestDatabaseHelper
{
    public static async Task InitializeAsync(AuthDbContext dbContext, IEnumerable<User> users)
    {
        await dbContext.Database.MigrateAsync();
        
        dbContext.Users.AddRange(users);
        await dbContext.SaveChangesAsync();
    }
    
    public static async Task ClearAsync(DbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"public\".\"Users\" CASCADE ;");
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"public\".\"ExternalAccounts\" CASCADE;");
    }

    public static AuthDbContext CreatePostgresDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseNpgsql(connectionString, x => 
                x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName))
            .Options;
        return new AuthDbContext(options);
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