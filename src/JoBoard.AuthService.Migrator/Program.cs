using JoBoard.AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        var connectionStr = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connectionStr, 
            x => x.MigrationsAssembly(typeof(Program).Assembly.FullName)));
    })
    .Build();

if (args.Contains("--migrate"))
{
    using var scope = host.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}