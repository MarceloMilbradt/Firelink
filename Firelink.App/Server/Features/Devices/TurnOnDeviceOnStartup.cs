using Firelink.Application.Devices.Commands.TurnOnAllDevices;
using MediatR;

namespace Firelink.App.Server.Features.Devices;

public class TurnOnDeviceOnStartup : BackgroundService
{
    private readonly IMediator _mediator;

    public TurnOnDeviceOnStartup(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _mediator.Send(TurnOnAllDevicesCommand.Default, stoppingToken);
    }
}