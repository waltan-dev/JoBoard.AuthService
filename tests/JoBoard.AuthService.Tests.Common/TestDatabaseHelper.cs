using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common.Fixtures;
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
        var users = new List<User>
        {
            DatabaseUserFixtures.ExistingActiveUser,
            DatabaseUserFixtures.ExistingUserRegisteredByEmail.Value,
            DatabaseUserFixtures.ExistingUserRegisteredByGoogleAccount.Value,
            DatabaseUserFixtures.ExistingUserWithExpiredToken.Value
        };
        dbContext.Users.AddRange(users);
        await dbContext.SaveChangesAsync();
    }
}