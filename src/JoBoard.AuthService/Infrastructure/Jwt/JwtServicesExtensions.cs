using JoBoard.AuthService.Application.Services;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public static class JwtServicesExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        
        UseJwtAuthentication(services);
        
        return services;
    }

    private static void UseJwtAuthentication(IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, JwtStartupFilter>();
    }
}