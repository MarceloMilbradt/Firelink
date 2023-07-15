using Firelink.Application.Common.Interfaces;
using MediatR;
using SpotifyAPI.Web;
using TuyaConnector.Data;

namespace Firelink.Application.Tracks.Events.ColorChanged;

internal sealed class ColorChangedNotificationHandler : INotificationHandler<ColorChangedNotification>
{
    private readonly ITuyaConnector _tuyaConnector;

    public ColorChangedNotificationHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task Handle(ColorChangedNotification notification, CancellationToken cancellationToken)
    {
        return;
        var color = notification.NewColor;
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = LedCommands.Color,
            Value = color,
        };

        await _tuyaConnector.SendCommandToAllDevices(command, WorkMode.Color, cancellationToken);
    }
}