using Firelink.App.Shared.Devices;
using Firelink.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firelink.Application.Devices.Queries.GetUserDevices;

public record GetUserDeviceQuery(string DeviceId) : IRequest<DeviceDto>;

internal sealed class GetUserDeviceQueryHandler : IRequestHandler<GetUserDeviceQuery, DeviceDto>
{
    private readonly ITuyaConnector _tuyaConnector;

    public GetUserDeviceQueryHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<DeviceDto> Handle(GetUserDeviceQuery request, CancellationToken cancellationToken)
    {
        var device = await _tuyaConnector.GetDevice(request.DeviceId, cancellationToken);
        return
            new DeviceDto
            {
                Id = device.Id,
                ProductName = device.ProductName,
                Name = device.Name,
                Online = Convert.ToBoolean(device.IsOnline)

            };
    }
}