using JoBoard.AuthService.Application.Commands.Register.ByEmail;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblyContaining<RegisterByEmailCommandHandler>());
        return services;
    }
}