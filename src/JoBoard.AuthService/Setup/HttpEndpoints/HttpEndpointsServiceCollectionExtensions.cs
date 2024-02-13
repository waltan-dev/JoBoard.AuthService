using JoBoard.AuthService.Middlewares.Filters;
using JoBoard.AuthService.Middlewares.Json;

namespace JoBoard.AuthService.Setup.HttpEndpoints;

internal static class HttpEndpointsServiceCollectionExtensions
{
    public static IServiceCollection AddHttpEndpoints(this IServiceCollection services)
    {
        services
            .AddControllers(options => options.Filters.Add<GlobalExceptionFilter>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new TrimStringJsonConverter());
            });
        
        UseHttpEndpoints(services);
        
        return services;
    }

    private static void UseHttpEndpoints(IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, HttpEndpointsStartupFilter>();
    }
}