using System.Globalization;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using JoBoard.AuthService.Application.Common.Behaviors;
using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using JoBoard.AuthService.Domain.Common.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, 
        ConfirmationTokenConfig confirmTokenConfig,
        RefreshTokenConfig refreshTokenConfig)
    {
        Guard.IsNotNull(confirmTokenConfig);
        Guard.IsNotDefault(confirmTokenConfig.ExpiresInHours);
        services.AddSingleton(confirmTokenConfig);
        
        Guard.IsNotNull(refreshTokenConfig);
        Guard.IsNotDefault(refreshTokenConfig.ExpiresInHours);
        services.AddSingleton(refreshTokenConfig);
        
        services.AddMediatR(config =>
        {
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.RegisterServicesFromAssemblyContaining<RegisterByEmailCommandHandler>();
        });
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        services.AddValidatorsFromAssembly(typeof(RegisterByEmailCommandHandler).Assembly);

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");
        
        return services;
    }
}