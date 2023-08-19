using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.CreateRainbowEffect;

public sealed record CreateRainbowEffectCommand : IRequest<bool>;


internal sealed class CreateRainbowEffectCommandHandler : IRequestHandler<CreateRainbowEffectCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;

    public CreateRainbowEffectCommandHandler(ITuyaConnector tuyaConnector)
    {
        _tuyaConnector = tuyaConnector;
    }
    public async Task<bool> Handle(CreateRainbowEffectCommand request, CancellationToken cancellationToken)
    {
        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var scene = new Scene
        {
            SceneNum = 102,
            SceneUnits = new[]
           {
                new SceneUnit(0, 1000, 1000, "gradient",  50, 50),
                new SceneUnit(60, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(120, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(180, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(240, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(300, 1000, 1000, "gradient", 50, 50),
            }
        };

        var command = new Command
        {
            Code = LedCommands.Scene,
            Value = scene,
        };


        var deviceTasks = devices.Select(device => _tuyaConnector.SendUpdateCommandToDevice(device.Id, new Command { Code = LedCommands.Scene, Value = scene }, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
        return true;
    }
}
