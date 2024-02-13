using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, GoogleAuthConfig googleAuthConfig)
    {
        Guard.IsNotNull(googleAuthConfig?.ClientId);
        //Guard.IsNotNull(googleAuthConfig?.ClientSecret);

        services.AddSingleton(googleAuthConfig);
        services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}