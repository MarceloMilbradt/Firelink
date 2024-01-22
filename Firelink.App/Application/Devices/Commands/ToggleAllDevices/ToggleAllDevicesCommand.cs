using Firelink.Application.Common.Interfaces;
using Mediator;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.ToggleAllDevices;

public record ToggleAllDevicesCommand(bool Power) : IRequest<bool>
{
    public static readonly ToggleAllDevicesCommand Default = new(true);
}

public sealed class ToggleAllDevicesCommandHandler : IRequestHandler<ToggleAllDevicesCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public ToggleAllDevicesCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async ValueTask<bool> Handle(ToggleAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = LedCommands.Power,
            Value = request.Power,
        };

        var deviceTasks = devices.Select(device => _tuyaConnector.SendUpdateCommandToDevice(device.Id, command, cancellationToken));
        await Task.WhenAll(deviceTasks);
        return true;
    }
}