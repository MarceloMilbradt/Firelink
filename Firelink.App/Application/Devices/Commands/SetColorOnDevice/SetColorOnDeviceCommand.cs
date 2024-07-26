using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Mediator;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.SetColorOnDevice;

public sealed record SetColorOnDeviceCommand(string DeviceId, Hsv Color) : IRequest<bool>;

public sealed class SetColorOnDeviceCommandHandler : IRequestHandler<SetColorOnDeviceCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public SetColorOnDeviceCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async ValueTask<bool> Handle(SetColorOnDeviceCommand request, CancellationToken cancellationToken)
    {
        var command = new Command
        {
            Code = LedCommands.Color,
            Value = request.Color,
        };

        await _tuyaConnector.SendCommandToDevice(request.DeviceId, command, cancellationToken);
        return true;
    }
}