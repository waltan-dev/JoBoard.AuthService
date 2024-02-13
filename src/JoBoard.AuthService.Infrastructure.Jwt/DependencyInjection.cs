using System.Text;
using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Infrastructure.Jwt.Configs;
using JoBoard.AuthService.Infrastructure.Jwt.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtInfrastructure(
        this IServiceCollection services, 
        JwtConfig jwtConfig,
        RedisConfig redisConfig)
    {
        ValidateJwtConfig(jwtConfig);

        services.AddSingleton(jwtConfig);
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, JwtIdentityService>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<JwtSignInManager>();

        services.AddStackExchangeRedisCache(options =>
        {
            var connectionString = $"{redisConfig.Host}:{redisConfig.Port},password={redisConfig.Password}";
            options.Configuration = connectionString;
        });
        services.AddScoped<IRefreshTokenRepository, RedisRefreshTokenRepository>();
        
        return services;
    }

    public static AuthenticationBuilder AddJwtAuthentication(
        this AuthenticationBuilder authenticationBuilder, 
        JwtConfig jwtConfig,
        Func<JwtBearerChallengeContext,Task>? onChallenge = null)
    {
        ValidateJwtConfig(jwtConfig);
        
        authenticationBuilder.AddJwtBearer(options =>
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

            if (onChallenge != null)
                options.Events = new JwtBearerEvents { OnChallenge = onChallenge };
        });
        return authenticationBuilder;
    }

    private static void ValidateJwtConfig(JwtConfig jwtConfig)
    {
        Guard.IsNotNull(jwtConfig);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.SecretKey);
        // SecretKey must be >= 256 bits (HS256)
        Guard.IsGreaterThanOrEqualTo(Encoding.UTF8.GetBytes(jwtConfig.SecretKey).Length * 8, 256);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.Issuer);
        Guard.IsNotNullOrWhiteSpace(jwtConfig.Audience);
        Guard.IsNotDefault(jwtConfig.TokenLifeSpan);
    }
}