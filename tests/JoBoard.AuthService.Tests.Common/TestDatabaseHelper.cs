﻿using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Tests.Common;

public static class TestDatabaseHelper
{
    public static void Initialize(AuthDbContext dbContext)
    {
        dbContext.Database.Migrate();
        AddUserFixtures(dbContext);
    }
    
    public static void Clear(DbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"Users\" CASCADE ;");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"ExternalAccounts\" CASCADE;");
    }

    private static void AddUserFixtures(AuthDbContext dbContext)
    {
        dbContext.Users.AddRange(
            DatabaseUserFixtures.ExistingActiveUser, 
            DatabaseUserFixtures.ExistingUserRegisteredByEmail, 
            DatabaseUserFixtures.ExistingUserRegisteredByGoogleAccount,
            DatabaseUserFixtures.ExistingUserWithExpiredToken
            );
        dbContext.SaveChanges();
    }
}