using Firelink.App.Shared;
using System.Threading.Channels;

namespace Firelink.Application.Common.Interfaces;

public interface ITrackChangeNotificationChannelProvider
{
    public Channel<TrackDto> Channel { get; }
}
