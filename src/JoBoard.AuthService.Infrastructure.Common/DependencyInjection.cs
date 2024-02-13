using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Common.Configs;
using JoBoard.AuthService.Infrastructure.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure.Common;

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