using JoBoard.AuthService.Application;
using JoBoard.AuthService.Infrastructure;
using JoBoard.AuthService.Infrastructure.Authentication;
using JoBoard.AuthService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// add controllers, swagger and other api infrastructure
services.AddApiInternalInfrastructure();

// add app services
services.AddHttpContextAccessor();

var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDatabase(connectionString);

var confirmTokenConfig = configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
services.AddInfrastructure(confirmTokenConfig);

services.AddApplication();

var app = builder.Build();
app.UseAuthorization();
app.Run();

namespace JoBoard.AuthService
{
    public partial class Program {}
} // only for tests