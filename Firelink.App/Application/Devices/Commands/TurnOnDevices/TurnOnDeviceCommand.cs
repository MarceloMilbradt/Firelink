using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.TurnOnDevices;

public record TurnOnDeviceCommand(string DeviceId) : IRequest<bool>;

internal sealed class TurnOnDeviceCommandHandler : IRequestHandler<TurnOnDeviceCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public TurnOnDeviceCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<bool> Handle(TurnOnDeviceCommand request, CancellationToken cancellationToken)
    {
        var command = new Command
        {
            Code = "switch_led",
            Value = true,
        };

        await _tuyaConnector.SendCommandToDevice(request.DeviceId, command, cancellationToken);
        return true;
    }
}