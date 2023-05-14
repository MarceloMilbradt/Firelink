using Firelink.Application.Tracks.Events.TrackChanged;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Firelink.App.Server.Features.Spotify.Track;


internal sealed class TrackChangedNotificationHandler : INotificationHandler<TrackChangedNotification>
{
    private readonly IHubContext<TrackHub> _trackHub;
    public TrackChangedNotificationHandler(IHubContext<TrackHub> trackHub)
    {
        _trackHub = trackHub;
    }

    public async Task Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        await TrackHub.SendTrackTo(_trackHub.Clients.All, notification.newTrack);
    }
}