using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Firelink.App.Server.Features.Spotify.Track;

public class TrackHubContext : ITrackHubContext
{
    private readonly IHubContext<TrackHub> _hubContext;

    public TrackHubContext(IHubContext<TrackHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendTrack(TrackDto trackDto)
    {
        await TrackHub.SendTrackTo(_hubContext.Clients.All, trackDto);
    }
}