using Firelink.App.Shared.Devices;
using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Devices.Queries.GetUserDevices;

public record GetUserDeviceQuery(string DeviceId) : IRequest<DeviceDto>;

public sealed class GetUserDeviceQueryHandler : IRequestHandler<GetUserDeviceQuery, DeviceDto>
{
    private readonly ITuyaConnector _tuyaConnector;

    public GetUserDeviceQueryHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async ValueTask<DeviceDto> Handle(GetUserDeviceQuery request, CancellationToken cancellationToken)
    {
        var device = await _tuyaConnector.GetDevice(request.DeviceId, cancellationToken);
        return new DeviceDto
        {
            Id = device.Id,
            ProductName = device.ProductName,
            Name = device.Name,
            Online = Convert.ToBoolean(device.IsOnline)

        };
    }
}