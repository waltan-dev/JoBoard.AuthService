using JoBoard.AuthService.Application;
using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Infrastructure.Auth;
using JoBoard.AuthService.Infrastructure.Auth.Configs;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.InternalInfrastructure.HttpEndpoints;
using JoBoard.AuthService.InternalInfrastructure.Jwt;
using JoBoard.AuthService.InternalInfrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);

// add internal infrastructure, e.g. controllers, jwt, swagger
var jwtConfig = builder.Configuration.GetRequiredSection(nameof(JwtConfig)).Get<JwtConfig>();
builder.Services
    .AddJwtAuthentication(jwtConfig)
    .AddHttpEndpoints()
    .AddSwagger();

// add app services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDatabaseInfrastructure(connectionString);

var googleAuthConfig = builder.Configuration.GetRequiredSection(nameof(GoogleAuthConfig)).Get<GoogleAuthConfig>();
var redisConfig = builder.Configuration.GetRequiredSection(nameof(RedisConfig)).Get<RedisConfig>();
builder.Services.AddAuthInfrastructure(googleAuthConfig, redisConfig);

var confirmTokenConfig = builder.Configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
builder.Services.AddApplication(confirmTokenConfig);

builder.Build().Run();