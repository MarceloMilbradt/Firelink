using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.TurnOnAllDevices;

public record TurnOnAllDevicesCommand() : IRequest<bool>;

internal sealed class TurnOnAllDevicesCommandHandler : IRequestHandler<TurnOnAllDevicesCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public TurnOnAllDevicesCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<bool> Handle(TurnOnAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = "switch_led",
            Value = true,
        };
        
        var deviceTasks = devices.Select(device => _tuyaConnector.SendCommandToDevice(device.Id, command, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
        return true;
    }
}