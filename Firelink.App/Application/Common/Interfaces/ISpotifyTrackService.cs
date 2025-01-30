using Firelink.Domain;

namespace Firelink.Application.Common.Interfaces;

public interface ISpotifyTrackService
{
    Task<TrackDto?> GetTrackWithColor(CancellationToken cancellationToken);
}