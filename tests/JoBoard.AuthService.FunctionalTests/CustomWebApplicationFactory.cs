using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.FunctionalTests;

public class CustomWebApplicationFactory : WebApplicationFactory<JoBoard.AuthService.Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("db_for_functional_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build(); 
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AuthDbContext>>();
            services.RemoveAll<AuthDbContext>();
            
            services.AddDbContext<AuthDbContext>(options => 
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString(), x => 
                    x.MigrationsAssembly(typeof(AuthService.Migrator.AssemblyReference).Assembly.FullName)));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        var host = base.CreateHost(builder);
        
        ResetDatabase(host.Services);

        return host;
    }
    
    private static void ResetDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        SeedData.Reinitialize(dbContext);
    }

    public Task InitializeAsync() // init test db before each test
    {
        return _postgreSqlContainer.StartAsync();
    }
    
    public new Task DisposeAsync() // delete test db after each test
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}