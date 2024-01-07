using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfirmationTokenConfig confirmTokenConfig)
    {
        Guard.IsNotNull(confirmTokenConfig);
        Guard.IsNotDefault(confirmTokenConfig.ExpiresInHours);
        
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        
        services.AddSingleton(confirmTokenConfig);
        services.AddSingleton<ITokenizer, Tokenizer>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}