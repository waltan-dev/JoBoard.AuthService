using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data;

public abstract class BaseRepositoryTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("db_for_integration_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build(); 
    
    protected AuthDbContext DbContext { get; private set; }
    protected UnitOfWork UnitOfWork { get; private set; }
    protected UserRepository UserRepository { get; private set; }
    
    public async Task InitializeAsync() // init test db before each test
    {
        await _postgreSqlContainer.StartAsync();
        
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString(), x => 
                x.MigrationsAssembly(typeof(AuthService.Migrator.AssemblyReference).Assembly.FullName))
            .Options;
        
        SeedData.Reinitialize(new AuthDbContext(options));
        
        DbContext = new AuthDbContext(options);
        UnitOfWork = new UnitOfWork(DbContext);
        UserRepository = new UserRepository(DbContext);
    }
    
    public Task DisposeAsync() // delete test db after each test
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}