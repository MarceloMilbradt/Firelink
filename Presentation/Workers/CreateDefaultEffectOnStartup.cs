using Firelink.Application.CustomEffects.Commands.CreateCustomEffect;
using Mediator;

namespace Firelink.Presentation.Workers;

public class CreateDefaultEffectOnStartup : BackgroundService
{
    private readonly ISender _sender;
    private readonly ILogger<CreateDefaultEffectOnStartup> _logger;
    public CreateDefaultEffectOnStartup(IMediator mediator, ILogger<CreateDefaultEffectOnStartup> logger)
    {
        _sender = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _sender.Send(new CreateDefaultEffectCommand(3, 0, 106, string.Empty), stoppingToken);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating default effect");
        }
    }
}