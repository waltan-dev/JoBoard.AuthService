using System.Diagnostics.CodeAnalysis;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace JoBoard.AuthService.Tests.Functional;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly SemaphoreSlim Semaphore = new(8); // max 8 parallel tests and docker databases
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase($"db_for_functional_tests-{Guid.NewGuid()}")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithName(Guid.NewGuid().ToString())
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // google
            services.RemoveAll<IGoogleAuthProvider>();
            services.AddSingleton(TestsRegistry.GoogleAuthProvider);
            
            // db
            services.RemoveAll<DbContextOptions<AuthDbContext>>();
            services.RemoveAll<AuthDbContext>();
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString(), x => 
                    x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
            });
            
            // redis
            services.RemoveAll<RedisCacheOptions>();
            services.RemoveAll<IDistributedCache>();
            services.AddStackExchangeRedisCache(options =>
            {
                var connectionString = _redisContainer.GetConnectionString();
                options.Configuration = connectionString;
            });
        });
    }
    
    public async Task InitializeAsync() // init one test db per one test file
    {
        await Semaphore.WaitAsync();

        await Task.WhenAll(
            _postgreSqlContainer.StartAsync(), 
            _redisContainer.StartAsync()
            );
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await TestDatabaseHelper.InitializeAsync(dbContext);
    }
    
    public new async Task DisposeAsync() // delete test db after all tests in one test file 
    {
        Semaphore.Release();
        
        await Task.WhenAll(
            _postgreSqlContainer.DisposeAsync().AsTask(), 
            _redisContainer.DisposeAsync().AsTask()
        );
    }
}