using Firelink.Application.Common.Interfaces;
using MediatR;

namespace Firelink.Application.Tracks.Events.TrackChanged;


internal sealed class TrackChangedNotificationHandler : INotificationHandler<TrackChangedNotification>
{
    private readonly ITrackHubContext _trackHub;
    public TrackChangedNotificationHandler(ITrackHubContext trackHub)
    {
        _trackHub = trackHub;
    }

    public async Task Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        await _trackHub.SendTrack(notification.newTrack);
    }
}