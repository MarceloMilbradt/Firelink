using Firelink.App.Shared;
using Firelink.Application.Tracks.Events.TrackChanged;
using Mediator;
using Serilog;

namespace Firelink.Presentation.Components.Track;

public partial class Track : INotificationHandler<TrackChangedNotification>
{
    public static event EventHandler<TrackDto> TrackChanged;
    public ValueTask Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        TrackChanged?.Invoke(this, notification.NewTrack);
        return ValueTask.CompletedTask;
    }
}
