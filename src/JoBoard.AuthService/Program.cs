using JoBoard.AuthService.Application;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Infrastructure;
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

services.AddInfrastructure();

var confirmTokenConfig = configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
services.AddApplication(confirmTokenConfig);

builder.Build().Run();

public partial class Program {} // only for tests