using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        
        services.Configure<ConfirmationTokenConfig>(configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)));
        services.AddSingleton<ITokenizer, Tokenizer>();
        
        return services;
    }
}