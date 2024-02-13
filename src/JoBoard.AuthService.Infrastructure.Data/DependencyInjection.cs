using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JoBoard.AuthService.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services, string connectionString)
    {
        Guard.IsNotNullOrWhiteSpace(connectionString);
        
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(connectionString, x => 
                x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
        });

        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IChangeTracker, EfChangeTracker>();

        return services;
    }
}