using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public static class JwtServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
    {
        Guard.IsNotNull(jwtConfig);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.SecretKey);
        // SecretKey must be >= 256 bits (HS256)
        Guard.IsGreaterThanOrEqualTo(Encoding.UTF8.GetBytes(jwtConfig.SecretKey).Length * 8, 256);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.Issuer);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.Audience);
        Guard.IsNotDefault(jwtConfig.TokenLifeSpan);
        Guard.IsNotDefault(jwtConfig.RefreshTokenLifeSpan);

        services.AddSingleton(jwtConfig);
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IIdentityService, JwtIdentityService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    
                    ValidateLifetime = true,
                    
                    IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        // Call this to skip the default logic and avoid using the default response
                        context.HandleResponse();

                        // Write to the response in any way you wish
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        var unauthorizedResponse = new UnauthorizedResponse { Message = "You are not authorized" };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(unauthorizedResponse));
                    }
                };
            });
        
        return services;
    }
}