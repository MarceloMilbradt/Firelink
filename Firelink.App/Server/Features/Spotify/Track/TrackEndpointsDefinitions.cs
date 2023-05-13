namespace Firelink.App.Server.Features.Spotify.Track;

public class TrackEndpointsDefinitions : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapHub<TrackHub>("/trackhub");

    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddTransient<SpotifyTrackService>();
        services.AddTransient<SpotifyTrackAnalyticsService>();
    }
}