using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Services;
using Firelink.Infrastructure.Services.Spotify;
using Firelink.Infrastructure.Services.Tuya;
using Microsoft.Extensions.DependencyInjection;

namespace Firelink.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {

        services.AddSingleton<ISpotifyApi, SpotifyApi>();
        services.AddTransient<IDateTime, DateTimeProvider>();
        services.AddTransient<IPlayerListenerService, PlayerListenerService>();
        services.AddTransient<ISpotifyTrackAnalyticsService, SpotifyTrackAnalyticsService>();
        services.AddTransient<ITuyaConnector,TuyaConnectorService>();
        services.AddLazyCache();
        return services;
    }
}