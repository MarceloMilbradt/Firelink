using Firelink.Application.Common.Configuration;
using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Services;
using Firelink.Infrastructure.Services.Spotify;
using Firelink.Infrastructure.Services.Wled;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Firelink.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<ISpotifyApi, SpotifyApi>();
        services.AddTransient<IDateTime, DateTimeProvider>();
        services.AddSingleton<IPlayerListenerService, PlayerListenerService>();
        services.AddTransient<ISpotifyTrackService, SpotifyTrackService>();
        services.AddSingleton<IAlbumColorProvider, AlbumColorProvider>();
        services.AddSingleton<IWledCustomEffectProvider, WledConfigurationProvider>();
        services.AddSingleton<IWledService, WledService>();
        services.AddLazyCache();
        var wledUrl = configuration.GetSection("Wled:Url").Value!;
        services.AddWLed(configure => configure.Url = wledUrl);
        services.Configure<LedColorConfiguration>(configuration.GetSection("LedConfiguration"));
        return services;
    }
}