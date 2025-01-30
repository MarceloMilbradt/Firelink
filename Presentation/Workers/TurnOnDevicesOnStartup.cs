using Firelink.Application.Devices.Commands.ToggleAllDevices;
using Mediator;

namespace Firelink.Presentation.Workers;

public class TurnOnDevicesOnStartup : BackgroundService
{
    private readonly ISender _sender;
    private readonly ILogger<TurnOnDevicesOnStartup> _logger;
    public TurnOnDevicesOnStartup(IMediator mediator, ILogger<TurnOnDevicesOnStartup> logger)
    {
        _sender = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var result = await _sender.Send(ToggleAllDevicesCommand.Default, stoppingToken);
        result.Switch(
            success => { },
            error => _logger.LogError("Error turning on devices {@Error}", error)
            );

    }
}
