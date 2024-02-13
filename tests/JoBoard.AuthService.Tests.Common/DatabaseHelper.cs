using JoBoard.AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common;

public static class DatabaseHelper
{
    public static void Reinitialize(AuthDbContext dbContext)
    {
        dbContext.Database.Migrate();
        ClearDatabase(dbContext);
        AddUserFixtures(dbContext);
    }

    private static void ClearDatabase(DbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"Users\" CASCADE ;");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"ExternalAccounts\" CASCADE;");
    }

    private static void AddUserFixtures(AuthDbContext dbContext)
    {
        dbContext.Users.AddRange(
            UserFixtures.ExistingActiveUser, 
            UserFixtures.ExistingUserRegisteredByEmail, 
            UserFixtures.ExistingUserRegisteredByGoogleAccount,
            UserFixtures.ExistingUserWithExpiredToken
            );
        dbContext.SaveChanges();
    }
}