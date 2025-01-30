using Firelink.Domain;

namespace Firelink.Application.Common.Interfaces;

public interface ITrackHubContext
{
    public Task SendTrack(TrackDto trackDto);
}