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
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 2,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
            .MinimumLevel.Information()
            .CreateLogger();

        services.AddSerilog(Log.Logger, dispose: true);
        services.AddMediator();

        services.AddHostedService<PlayerListener>();
        services.AddHostedService<TurnOnDevicesOnStartup>();
        services.AddHostedService<CreateDefaultEffectOnStartup>();
        return services;
    }
}