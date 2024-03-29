using JoBoard.AuthService.Application;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Infrastructure.Auth;
using JoBoard.AuthService.Infrastructure.Auth.Configs;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.Jwt.Configs;
using JoBoard.AuthService.Setup.HttpEndpoints;
using JoBoard.AuthService.Setup.Jwt;
using JoBoard.AuthService.Setup.Swagger;

var builder = WebApplication.CreateBuilder(args);

// add internal infrastructure, e.g. controllers, jwt, swagger
var jwtConfig = builder.Configuration.GetRequiredSection(nameof(JwtConfig)).Get<JwtConfig>();
var redisConfig = builder.Configuration.GetRequiredSection(nameof(RedisConfig)).Get<RedisConfig>();
builder.Services
    .AddJwtAuthentication(jwtConfig, redisConfig)
    .AddHttpEndpoints()
    .AddSwagger();

// add app services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDatabaseInfrastructure(connectionString);

var googleAuthConfig = builder.Configuration.GetRequiredSection(nameof(GoogleAuthConfig)).Get<GoogleAuthConfig>();
builder.Services.AddAuthInfrastructure(googleAuthConfig);

var confirmTokenConfig = builder.Configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
builder.Services.AddApplication(confirmTokenConfig);

builder.Build().Run();