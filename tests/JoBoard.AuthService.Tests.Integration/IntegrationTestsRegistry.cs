using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Common.Services;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Common.Stubs;
using JoBoard.AuthService.Tests.Integration.Fixtures;

namespace JoBoard.AuthService.Tests.Integration;

public static class IntegrationTestsRegistry
{
    public static IUserRepository GetUserRepository(string connectionString)
    {
        var dbContext = TestDatabaseHelper.CreatePostgresDbContext(connectionString);
        return new EfUserRepository(dbContext, new EfUnitOfWork(dbContext));
    }
    
    public static IUserEmailUniquenessChecker UserEmailUniquenessChecker 
        => new UserEmailUniquenessCheckerStubFactory().Create(DbUserFixtures.List);
    
    private static IPasswordHasher PasswordHasher 
        => new PasswordHasherStubFactory().Create();
    
    private static IPasswordStrengthValidator PasswordStrengthValidator 
        => new PasswordStrengthValidatorStubFactory().Create();
    
    private static ISecureTokenizer SecureTokenizer
        => new SecureTokenizerStubFactory().Create();
    
    public static ConfirmationTokenBuilder ConfirmationTokenBuilder 
        => new(SecureTokenizer);
    
    public static UserPasswordBuilder UserPasswordBuilder 
        => new(PasswordHasher, PasswordStrengthValidator);
    
    public static UserBuilder UserBuilder =>
        new(PasswordFixtures.DefaultPassword,
            ConfirmationTokenBuilder,
            PasswordHasher,
            PasswordStrengthValidator);
    
    public static IDateTime CurrentDateTime => new DateTimeProvider();
    public static IDateTime FutureDateTime => new TestDateTimeProvider(DateTime.UtcNow.AddDays(7));
}