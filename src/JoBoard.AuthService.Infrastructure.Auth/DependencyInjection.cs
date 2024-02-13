using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Auth.Configs;
using JoBoard.AuthService.Infrastructure.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, 
        GoogleAuthConfig googleAuthConfig)
    {
        Guard.IsNotNull(googleAuthConfig?.ClientId);
        //Guard.IsNotNull(googleAuthConfig?.ClientSecret);
        services.AddSingleton(googleAuthConfig);

        services.AddSingleton<IDateTime, DateTimeProvider>();
        
        services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPasswordStrengthValidator, PasswordStrengthValidator>();
        services.AddSingleton<ISecureTokenizer, SecureTokenizer>();
        
        return services;
    }
}