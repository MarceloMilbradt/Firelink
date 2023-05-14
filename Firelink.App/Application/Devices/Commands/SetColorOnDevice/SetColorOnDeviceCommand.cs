using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.SetColorOnDevice;

public record SetColorOnDeviceCommand(string DeviceId) : IRequest<bool>;

internal sealed class SetColorOnDeviceCommandHandler : IRequestHandler<SetColorOnDeviceCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public SetColorOnDeviceCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<bool> Handle(SetColorOnDeviceCommand request, CancellationToken cancellationToken)
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