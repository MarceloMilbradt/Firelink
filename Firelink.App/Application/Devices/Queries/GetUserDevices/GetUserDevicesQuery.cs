using Firelink.App.Shared.Devices;
using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Queries.GetUserDevices;

public record GetUserDevicesQuery : IRequest<IEnumerable<DeviceDto>>
{
    public static readonly GetUserDevicesQuery Default = new();
}

internal sealed class GetUserDevicesQueryHandler : IRequestHandler<GetUserDevicesQuery, IEnumerable<DeviceDto>>
{
    private readonly ITuyaConnector _tuyaConnector;

    public GetUserDevicesQueryHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<IEnumerable<DeviceDto>> Handle(GetUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        return devices.Select(device =>
            new DeviceDto
            {
                Id = device.Id,
                ProductName = device.ProductName,
                Name = device.Name,
                Online = Convert.ToBoolean(device.IsOnline),
                Power = Convert.ToBoolean(device.StatusList.FirstOrDefault(s => s.Code == "switch_led").Value)
            });
    }
}