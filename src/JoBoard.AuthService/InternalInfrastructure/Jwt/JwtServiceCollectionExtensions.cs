using System.Net.Mime;
using System.Text.Json;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Infrastructure.Jwt.Configs;
using JoBoard.AuthService.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JoBoard.AuthService.InternalInfrastructure.Jwt;

public static class JwtServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, 
        JwtConfig jwtConfig,
        RedisConfig redisConfig)
    {
        services.AddJwtInfrastructure(jwtConfig, redisConfig);
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtAuthentication(jwtConfig, onChallenge: async context =>
            {
                // Call this to skip the default logic and avoid using the default response
                context.HandleResponse();

                // Write to the response in any way you wish
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var unauthorizedResponse = new UnauthorizedResponse { Message = "You are not authorized" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(unauthorizedResponse));
            });
        services.AddAuthorization();
        
        return services;
    }
}