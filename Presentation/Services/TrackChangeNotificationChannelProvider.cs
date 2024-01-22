using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using System.Threading.Channels;

namespace Firelink.Presentation.Services;

public class TrackChangeNotificationChannelProvider : ITrackChangeNotificationChannelProvider
{
    public TrackChangeNotificationChannelProvider()
    {
        Channel = System.Threading.Channels.Channel.CreateUnbounded<TrackDto>(new UnboundedChannelOptions { SingleWriter = true });
    }
    public Channel<TrackDto> Channel { get; private set; }
}
