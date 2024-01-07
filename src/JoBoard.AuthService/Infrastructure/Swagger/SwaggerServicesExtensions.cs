using System.Reflection;
using Microsoft.OpenApi.Models;

namespace JoBoard.AuthService.Infrastructure.Swagger;

internal static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly().GetName();
        var assemblyVersion = assembly.Version?.ToString() ?? "0.0.0";
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(assembly.Name, new OpenApiInfo
            {
                Title = $"{assembly.Name}.{assemblyVersion}", 
                Version = "v1"
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