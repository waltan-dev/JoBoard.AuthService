using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JoBoard.AuthService.Tests.Functional.Tests.API;

public abstract class BaseApiTest : 
    IClassFixture<CustomWebApplicationFactory>, 
    IClassFixture<DatabaseFixture>, 
    IClassFixture<RedisFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    protected readonly HttpClient HttpClient;
    protected readonly IUserRepository UserRepository;

    protected BaseApiTest(CustomWebApplicationFactory webApplicationFactory, DatabaseFixture databaseFixture, RedisFixture redisFixture)
    {
        _webApplicationFactory = webApplicationFactory.WithWebHostBuilder(
            x => AddInitializedContainers(x, databaseFixture, redisFixture));
        
        HttpClient = _webApplicationFactory.CreateClient();
        
        var scope = _webApplicationFactory.Services.CreateScope();
        UserRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    }
    
    private static void AddInitializedContainers(
        IWebHostBuilder webHostBuilder, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture)
    {
        webHostBuilder.ConfigureTestServices(services =>
        {
            // google
            services.RemoveAll<IGoogleAuthProvider>();
            services.AddSingleton(FunctionalTestsRegistry.GoogleAuthProvider);
            
            // db
            services.RemoveAll<DbContextOptions<AuthDbContext>>();
            services.RemoveAll<AuthDbContext>();
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(databaseFixture.ConnectionString, x => 
                    x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
            });
            
            // redis
            services.RemoveAll<RedisCacheOptions>();
            services.RemoveAll<IDistributedCache>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisFixture.ConnectionString;
            });
        });
    }

    protected WebApplicationFactory<Program> GetServerWithFutureDate()
    {
        return _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IDateTime>();
                services.AddSingleton<IDateTime>(FunctionalTestsRegistry.FutureDateTime);
            });
        });
    }
}