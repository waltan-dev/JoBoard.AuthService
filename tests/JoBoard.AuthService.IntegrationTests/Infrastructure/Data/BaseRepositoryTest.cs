using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data;

public abstract class BaseRepositoryTest : IAsyncLifetime
{
    private static readonly SemaphoreSlim Semaphore = new(8); // max 8 parallel tests and docker databases
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase($"db_for_integration_tests-{Guid.NewGuid()}")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();
    
    protected IUnitOfWork UnitOfWork { get; private set; }
    protected IUserRepository UserRepository { get; private set; }
    
    public async Task InitializeAsync() // init test db before each test file
    {
        await Semaphore.WaitAsync();
        
        await _postgreSqlContainer.StartAsync();
        
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString(), x => 
                x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName))
            .Options;
        
        await TestDatabaseHelper.InitializeAsync(new AuthDbContext(options));
        
        var dbContext = new AuthDbContext(options);
        UnitOfWork = new EfUnitOfWork(dbContext);
        UserRepository = new EfUserRepository(dbContext);
    }
    
    public Task DisposeAsync() // delete test db after each test file
    {
        Semaphore.Release();
        
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}