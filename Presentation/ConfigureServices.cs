using Firelink.Application.Common.Interfaces;
using Firelink.Presentation.Workers;
using Mediator;
using Serilog;
using Serilog.Formatting.Compact;
namespace Firelink.Presentation;
public static class ConfigureServices
{

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddMediator( );

        services.AddHostedService<PlayerListener>();
        services.AddHostedService<TurnOnDevicesOnStartup>();
        services.AddHostedService<CreateDefaultEffectOnStartup>();
        return services;
    }
}