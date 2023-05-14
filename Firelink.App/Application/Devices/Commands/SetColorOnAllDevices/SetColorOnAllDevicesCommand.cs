using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.SetColorOnAllDevices;

public record SetColorOnAllDevicesCommand(Hsv Color) : IRequest<bool>;


internal class SetColorOnAllDevicesCommandHandler : IRequestHandler<SetColorOnAllDevicesCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public SetColorOnAllDevicesCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<bool> Handle(SetColorOnAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = "colour_data",
            Value = request.Color,
        };
        
        var deviceTasks = devices.Select(device => _tuyaConnector.SendCommandToDevice(device.Id, command, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
        return true;
    }
}