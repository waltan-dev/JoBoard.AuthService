using JoBoard.AuthService.Infrastructure.Http;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Infrastructure.Swagger;

namespace JoBoard.AuthService.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiInternalInfrastructure(this IServiceCollection services)
    {
        return services
            .AddJwtAuthentication()
            .AddHttp()
            .AddSwagger();
    }
}