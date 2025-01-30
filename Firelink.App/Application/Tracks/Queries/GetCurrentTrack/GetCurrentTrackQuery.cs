using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Result;
using Firelink.Domain;
using Mediator;
using OneOf.Types;
using OneOf;
using SpotifyAPI.Web;

namespace Firelink.Application.Tracks.Queries.GetCurrentTrack;


public sealed record GetCurrentTrackQuery : IRequest<OneOf<TrackDto?, None>>
{
    public static readonly GetCurrentTrackQuery Default = new();
}

public sealed class GetCurrentTrackQueryHandler : IRequestHandler<GetCurrentTrackQuery, OneOf<TrackDto?, None>>
{
    private readonly ISpotifyTrackService _trackAnalyticsService;

    public GetCurrentTrackQueryHandler(ISpotifyTrackService trackAnalyticsService)
    {
        _trackAnalyticsService = trackAnalyticsService;
    }

    public async ValueTask<OneOf<TrackDto?, None>> Handle(GetCurrentTrackQuery request, CancellationToken cancellationToken)
    {
        var currentTrack = await _trackAnalyticsService.GetTrackWithColor(cancellationToken);
        if (currentTrack == null)
        {
            return new None();
        }
        return currentTrack;
    }
}

