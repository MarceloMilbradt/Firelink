using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Result;
using Mediator;
using OneOf;
using OneOf.Types;
using SpotifyAPI.Web;

namespace Firelink.Application.Tracks.Queries.GetCurrentFullTrack;

public sealed record GetCurrentFullTrackQuery : IRequest<OneOf<FullTrack?, None, SpotifyError>>
{
    public static readonly GetCurrentFullTrackQuery Default = new();
}

public sealed class GetCurrentFullTrackQueryHandler : IRequestHandler<GetCurrentFullTrackQuery, OneOf<FullTrack?, None, SpotifyError>>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetCurrentFullTrackQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public async ValueTask<OneOf<FullTrack?, None, SpotifyError>> Handle(GetCurrentFullTrackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var currentTrack = await _spotifyApi.GetCurrentTrack(cancellationToken);
            if (currentTrack == null)
            {
                return new None();
            }
            return currentTrack;
        }
        catch (Exception ex)
        {
            return new SpotifyError
            {
                message = ex.Message,
            };
        }
    }
}