using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using MediatR;
using SpotifyAPI.Web;

namespace Firelink.Application.Tracks.Queries.GetCurrentTrack;


public sealed record GetCurrentTrackQuery : IRequest<TrackDto>
{
    public static readonly GetCurrentTrackQuery Default = new();
}

internal sealed   class GetCurrentTrackQueryHandler : IRequestHandler<GetCurrentTrackQuery, TrackDto>
{
    private readonly ISpotifyTrackAnalyticsService _trackAnalyticsService;

    public GetCurrentTrackQueryHandler(ISpotifyTrackAnalyticsService trackAnalyticsService)
    {
        _trackAnalyticsService = trackAnalyticsService;
    }

    public async Task<TrackDto> Handle(GetCurrentTrackQuery request, CancellationToken cancellationToken)
    {
        var currentTrack = await _trackAnalyticsService.GetTrackWithFeatures(cancellationToken);
        return currentTrack;
    }
}