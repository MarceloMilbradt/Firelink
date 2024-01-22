using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Tracks.Events.TrackChanged;

public sealed record TrackChangedNotification(TrackDto NewTrack) : INotification
{
}

public sealed class TrackChangedNotificationHandler : INotificationHandler<TrackChangedNotification>
{
    private readonly ITrackChangeNotificationChannelProvider _changeNotificationChannelProvider;

    public TrackChangedNotificationHandler(ITrackChangeNotificationChannelProvider changeNotificationChannelProvider)
    {
        _changeNotificationChannelProvider = changeNotificationChannelProvider;
    }

    public async ValueTask Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        await _changeNotificationChannelProvider.Channel.Writer.WriteAsync(notification.NewTrack);
    }
}
