using FluentValidation;
using JoBoard.AuthService.Application.Auth.Register.ByEmail;
using JoBoard.AuthService.Application.Core.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.RegisterServicesFromAssemblyContaining<RegisterByEmailCommandHandler>();
        });
        services.AddValidatorsFromAssembly(typeof(RegisterByEmailCommandHandler).Assembly);
        return services;
    }
}