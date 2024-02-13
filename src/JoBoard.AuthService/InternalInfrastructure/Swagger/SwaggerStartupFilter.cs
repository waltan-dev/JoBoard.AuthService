using Microsoft.AspNetCore.Rewrite;

namespace JoBoard.AuthService.InternalInfrastructure.Swagger;

internal class SwaggerStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                
                var rewriteOptions = new RewriteOptions()
                    .AddRedirect("^$", "/swagger/index.html");
                app.UseRewriter(rewriteOptions);
            }
            
            next(app);
        };
    }
}