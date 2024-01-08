using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}