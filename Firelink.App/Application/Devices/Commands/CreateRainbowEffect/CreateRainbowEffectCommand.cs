using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Mediator;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.CreateRainbowEffect;

public sealed record CreateRainbowEffectCommand : IRequest<bool>;


public sealed class CreateRainbowEffectCommandHandler : IRequestHandler<CreateRainbowEffectCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;
    private readonly IPlayerListenerService _playerListenerService;

    public CreateRainbowEffectCommandHandler(ITuyaConnector tuyaConnector, IPlayerListenerService playerListenerService)
    {
        _tuyaConnector = tuyaConnector;
        _playerListenerService = playerListenerService;
    }
    public async ValueTask<bool> Handle(CreateRainbowEffectCommand request, CancellationToken cancellationToken)
    {
        _playerListenerService.SetListen(false);

        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var scene = ScenePresets.Rainbow;

        var command = new Command
        {
            Code = LedCommands.Scene,
            Value = scene,
        };

        var deviceTasks = devices.Select(device => _tuyaConnector.SendUpdateCommandToDevice(device.Id, new Command { Code = LedCommands.Scene, Value = scene }, cancellationToken));
        await Task.WhenAll(deviceTasks);
        return true;
    }
}
