using Firelink.Application.Common.Interfaces;
using Firelink.Presentation.Services;
using Firelink.Presentation.Workers;
using Serilog;
namespace Firelink.Presentation;
public static class ConfigureServices
{

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 2)
            .CreateLogger();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));
        services.AddMediator();

        services.AddHostedService<PlayerListener>();
        services.AddHostedService<TurnOnDevicesOnStartup>();
        services.AddSingleton<ITrackChangeNotificationChannelProvider, TrackChangeNotificationChannelProvider>();
        return services;
    }
}