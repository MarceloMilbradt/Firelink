using Firelink.Application.Common.Interfaces;
using Mediator;
using SpotifyAPI.Web;

namespace Firelink.Application.Tracks.Queries.GetCurrentFullTrack;

public sealed record GetCurrentFullTrackQuery : IRequest<FullTrack?>
{
    public static readonly GetCurrentFullTrackQuery Default = new();
}

public sealed  class GetCurrentFullTrackQueryHandler : IRequestHandler<GetCurrentFullTrackQuery, FullTrack?>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetCurrentFullTrackQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public async ValueTask<FullTrack?> Handle(GetCurrentFullTrackQuery request, CancellationToken cancellationToken)
    {
        var currentTrack = await _spotifyApi.GetCurrentTrack(cancellationToken);
        return currentTrack;
    }
}