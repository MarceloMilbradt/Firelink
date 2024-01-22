using Firelink.App.Shared;
using Firelink.Application.Tracks.Queries.GetCurrentTrack;
using Mediator;
using Microsoft.AspNetCore.SignalR;

namespace Firelink.App.Server.Features.Spotify.Track;

public class TrackHub : Hub
{
    private readonly IMediator _mediator;
    public TrackHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        await SendTrack(await _mediator.Send(GetCurrentTrackQuery.Default));
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