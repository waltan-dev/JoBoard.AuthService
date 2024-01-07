using JoBoard.AuthService.Infrastructure.Filters;
using JoBoard.AuthService.Infrastructure.Json;

namespace JoBoard.AuthService.Infrastructure.Http;

internal static class HttpServicesExtensions
{
    public static IServiceCollection AddHttp(this IServiceCollection services)
    {
        services
            .AddControllers(options => options.Filters.Add<GlobalExceptionFilter>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new TrimStringJsonConverter());
            });
        
        UseHttp(services);
        
        return services;
    }

    private static void UseHttp(IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, HttpStartupFilter>();
    }
}