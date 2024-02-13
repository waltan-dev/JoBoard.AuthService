using System.Reflection;
using Microsoft.OpenApi.Models;

namespace JoBoard.AuthService.InternalInfrastructure.Swagger;

internal static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly().GetName();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = assembly.Name, Version = "v1" });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
                
            options.CustomSchemaIds(x => x.FullName);
                
            var xmlFileName = assembly.Name + ".xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);
        });
        
        UseSwagger(services);
        
        return services;
    }

    private static void UseSwagger(IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
    }
}