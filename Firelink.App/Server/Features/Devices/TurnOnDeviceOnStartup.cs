using Firelink.Application.Devices.Commands.ToggleAllDevices;
using Mediator;

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
        try
        {
            await _mediator.Send(ToggleAllDevicesCommand.Default, stoppingToken);

        }
        catch (Exception ex)
        {

        }
    }
}