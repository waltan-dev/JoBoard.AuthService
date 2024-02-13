using JoBoard.AuthService.Tests.Common;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.Tests.Functional.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase($"db_for_functional_tests-{Guid.NewGuid()}")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();
    
    public string ConnectionString { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        ConnectionString = _postgreSqlContainer.GetConnectionString();

        var dbContext = TestDatabaseHelper.CreatePostgresDbContext(ConnectionString);
        await TestDatabaseHelper.InitializeAsync(dbContext, DbUserFixtures.List);
    }
    
    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}