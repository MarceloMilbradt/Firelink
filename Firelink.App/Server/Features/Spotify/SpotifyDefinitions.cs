namespace Firelink.App.Server.Features.Spotify;

public class SpotifyDefinitions : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<ISpotifyAPI, SpotifyApi>();
    }
}

