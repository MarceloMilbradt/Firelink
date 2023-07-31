using Firelink.App.Server.Extensions;

namespace Firelink.App.Server.Features.Spotify.Track;

public class TrackEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapHub<TrackHub>("/trackhub");

    }
}