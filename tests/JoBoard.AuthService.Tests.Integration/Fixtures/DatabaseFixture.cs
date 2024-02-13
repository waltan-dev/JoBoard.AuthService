using JoBoard.AuthService.Tests.Common;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.Tests.Integration.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase($"db_for_integration_tests-{Guid.NewGuid()}")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();
    
    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync() // init test db before each test file
    {
        await _postgreSqlContainer.StartAsync();
        ConnectionString = _postgreSqlContainer.GetConnectionString();
        
        var dbContext = TestDatabaseHelper.CreatePostgresDbContext(ConnectionString);
        await TestDatabaseHelper.InitializeAsync(dbContext, DbUserFixtures.List);
    }
    
    public Task DisposeAsync() // delete test db after each test file
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}