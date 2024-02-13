﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace JoBoard.AuthService.FunctionalTests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly SemaphoreSlim Semaphore = new(4); // max 4 parallel tests and docker databases
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase($"db_for_functional_tests-{Guid.NewGuid()}")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build(); 

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // google
            services.RemoveAll<IGoogleAuthProvider>();
            services.AddSingleton<IGoogleAuthProvider>(GoogleFixtures.GetGoogleAuthProvider());
            
            // db
            services.RemoveDatabase();
            services.AddDatabaseInfrastructure(_postgreSqlContainer.GetConnectionString());
            Debug.WriteLine(_postgreSqlContainer.GetConnectionString());
        });
    }
    
    public async Task InitializeAsync() // init one test db per one test file
    {
        await Semaphore.WaitAsync();
        
        await _postgreSqlContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        TestDatabaseHelper.Initialize(dbContext);
    }
    
    public new Task DisposeAsync() // delete test db after all tests in one test file 
    {
        Semaphore.Release();
        
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}