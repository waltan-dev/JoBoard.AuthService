using JoBoard.AuthService.Application;
using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Infrastructure.Auth;
using JoBoard.AuthService.Infrastructure.Auth.Configs;
using JoBoard.AuthService.Infrastructure.Data;
using JoBoard.AuthService.Infrastructure.HttpEndpoints;
using JoBoard.AuthService.Infrastructure.Jwt;
using JoBoard.AuthService.Infrastructure.Swagger;

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
builder.Services.AddAuthInfrastructure(googleAuthConfig);

var confirmTokenConfig = builder.Configuration.GetRequiredSection(nameof(ConfirmationTokenConfig)).Get<ConfirmationTokenConfig>();
var refreshTokenConfig = builder.Configuration.GetRequiredSection(nameof(RefreshTokenConfig)).Get<RefreshTokenConfig>();
builder.Services.AddApplication(confirmTokenConfig, refreshTokenConfig);

builder.Build().Run();