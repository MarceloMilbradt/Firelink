using Firelink.App.Shared;

namespace Firelink.Application.Common.Interfaces;

public interface ISpotifyTrackAnalyticsService
{
    Task<TrackDto?> GetTrackWithFeatures(CancellationToken cancellationToken);
}