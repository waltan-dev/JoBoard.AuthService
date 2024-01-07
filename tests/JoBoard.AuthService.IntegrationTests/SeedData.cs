using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.IntegrationTests.API.Controllers.AccountV1;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.IntegrationTests;

public static class SeedData
{
    public static void Reinitialize(AuthDbContext dbContext)
    {
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
            RegisterFixtures.ExistingUserRegisteredByEmail, 
            RegisterFixtures.ExistingUserRegisteredByExternalAccount,
            RegisterFixtures.ExistingUserWithExpiredToken
            );
        dbContext.SaveChanges();
    }
}