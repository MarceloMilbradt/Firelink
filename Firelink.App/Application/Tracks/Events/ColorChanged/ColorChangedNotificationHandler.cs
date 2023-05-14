using Firelink.Application.Common.Interfaces;
using MediatR;
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
        var color = notification.NewColor;
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = "colour_data",
            Value = color,
        };

        var deviceTasks = devices
            .Select(device => _tuyaConnector.SendCommandToDevice(device.Id!, command, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
    }
}