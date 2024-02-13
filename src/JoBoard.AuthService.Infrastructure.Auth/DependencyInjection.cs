using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Configs;
using JoBoard.AuthService.Infrastructure.Auth.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace JoBoard.AuthService.Infrastructure.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, 
        GoogleAuthConfig googleAuthConfig,
        RedisConfig redisConfig)
    {
        Guard.IsNotNull(googleAuthConfig?.ClientId);
        //Guard.IsNotNull(googleAuthConfig?.ClientSecret);
        services.AddSingleton(googleAuthConfig);
        
        services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPasswordStrengthValidator, PasswordStrengthValidator>();
        services.AddSingleton<ISecureTokenizer, SecureTokenizer>();

        services.AddScoped<IConnectionMultiplexer>(cfg =>
        {
            var connectionString = $"{redisConfig.Host}:{redisConfig.Port},password={redisConfig.Password}";
            return ConnectionMultiplexer.Connect(connectionString);
        });
        services.AddScoped<IRefreshTokenRepository, RedisRefreshTokenRepository>();
        
        return services;
    }
}