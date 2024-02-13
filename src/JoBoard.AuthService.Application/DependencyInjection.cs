using System.Globalization;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using JoBoard.AuthService.Application.Behaviors;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfirmationTokenConfig confirmTokenConfig)
    {
        Guard.IsNotNull(confirmTokenConfig);
        Guard.IsNotDefault(confirmTokenConfig.ExpiresInHours);
        services.AddSingleton(confirmTokenConfig);
        
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