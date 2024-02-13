using JoBoard.AuthService.Application;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Infrastructure;
using JoBoard.AuthService.Infrastructure.Authentication;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.HttpEndpoints;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Infrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var jwtConfig = configuration.GetRequiredSection(nameof(JwtConfig)).Get<JwtConfig>();
services
    .AddJwtAuthentication(jwtConfig)
    .AddHttpEndpoints()
    .AddSwagger();

// add app services
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDatabase(connectionString);

var googleAuthConfig = configuration.GetRequiredSection(nameof(GoogleAuthConfig)).Get<GoogleAuthConfig>();
services.AddInfrastructure(googleAuthConfig);

var confirmTokenConfig = configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
services.AddApplication(confirmTokenConfig);

builder.Build().Run();

namespace JoBoard.AuthService
{
    public partial class Program {} // only for tests
}