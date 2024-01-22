using Firelink.App.Client.Components;
using Firelink.App.Server.Extensions;
using Firelink.App.Shared;
using Firelink.App.Shared.Devices;
using Firelink.Application.Devices.Commands.CreateRainbowEffect;
using Firelink.Application.Devices.Commands.SyncWithMusic;
using Firelink.Application.Devices.Commands.ToggleDevices;
using Firelink.Application.Devices.Queries.GetUserDevices;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web.Http;
using TuyaConnector.Data;

namespace Firelink.App.Server.Features.Devices;

public class DeviceEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var endpointGroup = app.MapGroup("devices");
        endpointGroup.MapGet("", GetDevices);
        endpointGroup.MapPatch("{id}", ToggleDevices);
        endpointGroup.MapPost("rainbow", SetAsRainbow);
        endpointGroup.MapPost("musicsync", SetAsSync);
    }

    private async Task<Results<Ok,BadRequest>> SetAsRainbow(IMediator mediator)
    {
        await mediator.Send(request: new CreateRainbowEffectCommand());
        return TypedResults.Ok();
    }
    private async Task<Results<Ok, BadRequest>> SetAsSync(IMediator mediator)
    {
        await mediator.Send(request: new SyncMusicCommand());
        return TypedResults.Ok();
    }
    private async Task<Results<Ok<ResultResponse<DeviceDto>>, BadRequest>> ToggleDevices(IMediator mediator, [FromRoute] string id, [FromBody] ToggleDeviceCommand request)
    {
        var device = await mediator.Send(request: new ToggleDeviceCommand(id, request.Power));
        return TypedResults.Ok(ResultResponse<DeviceDto>.Ok(device));
    }

    private async Task<Results<Ok<ResultResponse<IEnumerable<DeviceDto>>>, BadRequest>> GetDevices(IMediator mediator)
    {
        var devices = await mediator.Send(GetUserDevicesQuery.Default);
        return TypedResults.Ok(ResultResponse<IEnumerable<DeviceDto>>.Ok(devices));
    }
}