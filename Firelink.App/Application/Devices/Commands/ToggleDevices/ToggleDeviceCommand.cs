using Firelink.App.Shared.Devices;
using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.ToggleDevices;

public record ToggleDeviceCommand(string DeviceId, bool Power) : IRequest<DeviceDto>
{
}

internal sealed class ToggleDeviceCommandHandler : IRequestHandler<ToggleDeviceCommand, DeviceDto>
{
    private readonly ITuyaConnector _tuyaConnector;

    public ToggleDeviceCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<DeviceDto> Handle(ToggleDeviceCommand request, CancellationToken cancellationToken)
    {
        var command = new Command
        {
            Code = LedCommands.Power,
            Value = request.Power,
        };

        var device = await _tuyaConnector.SendUpdateCommandToDevice(request.DeviceId, command, cancellationToken);
        return new DeviceDto
        {
            Id = device.Id,
            ProductName = device.ProductName,
            Name = device.Name,
            Online = Convert.ToBoolean(device.IsOnline),
            Power = Convert.ToBoolean(device.StatusList.FirstOrDefault(s => s.Code == command.Code).Value)
        };
    }
}