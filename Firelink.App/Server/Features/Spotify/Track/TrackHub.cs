using Firelink.App.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Firelink.App.Server.Features.Spotify.Track;

public class TrackHub : Hub
{
    private readonly SpotifyTrackAnalyticsService _trackAnalyticsService;
    public TrackHub(SpotifyTrackAnalyticsService trackAnalyticsService)
    {
        _trackAnalyticsService = trackAnalyticsService;
    }

    public override async Task OnConnectedAsync()
    {
        await SendTrack(await _trackAnalyticsService.GetTrack());
        await base.OnConnectedAsync();
    }
    public static async Task SendTrackTo(IClientProxy clients, TrackDto track)
    {
        await clients.SendAsync("TrackChange", track);
    }
    public async Task SendTrack(TrackDto track)
    {
        await SendTrackTo(Clients.Caller, track);
    }
}