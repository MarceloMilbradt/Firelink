using Firelink.App.Client.Components;
using Firelink.App.Server.Extensions;
using Firelink.App.Shared;
using Firelink.App.Shared.Devices;
using Firelink.Application.Devices.Commands.ToggleDevices;
using Firelink.Application.Devices.Queries.GetUserDevices;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TuyaConnector.Data;

namespace Firelink.App.Server.Features.Devices;

public class DeviceEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/devices", GetDevices);
        app.MapPatch("/device/{id}", ToggleDevices);
    }

    private async Task<Ok<ResultResponse<DeviceDto>>> ToggleDevices(IMediator mediator, [FromRoute] string id, [FromBody] ToggleDeviceCommand request)
    {
        var device = await mediator.Send(new ToggleDeviceCommand(id, request.Power));
        return TypedResults.Ok(new ResultResponse<DeviceDto>(device, true));
    }

    private async Task<Ok<ResultResponse<IEnumerable<DeviceDto>>>> GetDevices(IMediator mediator)
    {
        var devices = await mediator.Send(GetUserDevicesQuery.Default);
        return TypedResults.Ok(new ResultResponse<IEnumerable<DeviceDto>>(devices, true));
    }
}