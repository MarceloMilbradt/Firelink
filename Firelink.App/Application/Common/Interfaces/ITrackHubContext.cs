using Firelink.App.Shared;

namespace Firelink.Application.Common.Interfaces;

public interface ITrackHubContext
{
    public Task SendTrack(TrackDto trackDto);
}