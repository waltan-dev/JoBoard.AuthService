using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Guard = CommunityToolkit.Diagnostics.Guard;

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

        services.AddScoped<IUserEmailUniquenessChecker, EfUserEmailUniquenessChecker>();
        services.AddScoped<IExternalAccountUniquenessChecker, EfExternalAccountUniquenessChecker>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, EfDomainEventDispatcher>();

        return services;
    }
}