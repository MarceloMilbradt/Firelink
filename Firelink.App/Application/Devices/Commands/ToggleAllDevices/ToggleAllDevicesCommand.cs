using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.ToggleAllDevices;

public record ToggleAllDevicesCommand(bool Power) : IRequest<bool>
{
    public static readonly ToggleAllDevicesCommand Default = new(true);
}

internal sealed class ToggleAllDevicesCommandHandler : IRequestHandler<ToggleAllDevicesCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public ToggleAllDevicesCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<bool> Handle(ToggleAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = "switch_led",
            Value = request.Power,
        };

        var deviceTasks = devices.Select(device => _tuyaConnector.SendUpdateCommandToDevice(device.Id, command, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
        return true;
    }
}