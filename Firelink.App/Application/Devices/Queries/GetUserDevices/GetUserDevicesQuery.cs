using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Queries.GetUserDevices;

public record GetUserDevicesQuery : IRequest<IEnumerable<Device>>;

internal sealed class GetUserDevicesQueryHandler : IRequestHandler<GetUserDevicesQuery, IEnumerable<Device>>
{
    private readonly ITuyaConnector _tuyaConnector;

    public GetUserDevicesQueryHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }

    public async Task<IEnumerable<Device>> Handle(GetUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        return devices;
    }
}