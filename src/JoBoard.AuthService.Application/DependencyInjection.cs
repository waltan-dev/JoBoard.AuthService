using System.Globalization;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using JoBoard.AuthService.Application.Behaviors;
using JoBoard.AuthService.Application.Commands.Register.ByEmailAndPassword;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, 
        ConfirmationTokenConfig confirmTokenConfig)
    {
        Guard.IsNotNull(confirmTokenConfig);
        Guard.IsNotDefault(confirmTokenConfig.TokenLifeSpan);
        services.AddSingleton(confirmTokenConfig);
        
        services.AddMediatR(config =>
        {
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.RegisterServicesFromAssemblyContaining<RegisterByEmailAndPasswordCommandHandler>();
        });
        
        services.AddValidatorsFromAssembly(typeof(RegisterByEmailAndPasswordCommandHandler).Assembly);

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");
        
        return services;
    }
}