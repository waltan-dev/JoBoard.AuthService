using JoBoard.AuthService.Infrastructure.Http;
using JoBoard.AuthService.Infrastructure.Swagger;

namespace JoBoard.AuthService.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiInternalInfrastructure(this IServiceCollection services)
    {
        return services
            .AddHttp()
            .AddSwagger();
    }
}