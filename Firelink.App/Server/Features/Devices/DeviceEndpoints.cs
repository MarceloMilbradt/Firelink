using Firelink.App.Shared;
using Firelink.App.Shared.Devices;
using Firelink.Application.Devices.Queries.GetUserDevices;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TuyaConnector.Data;

namespace Firelink.App.Server.Features.Devices;

public class DeviceEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/devices", GetDevices);
    }

    private async Task<Ok<ResultResponse<IEnumerable<DeviceDto>>>> GetDevices(IMediator mediator)
    {
        var devices = await mediator.Send(GetUserDevicesQuery.Default);
        return TypedResults.Ok(new ResultResponse<IEnumerable<DeviceDto>>(devices, true));
    }
}