using Firelink.Application.Common.Interfaces;
using Firelink.Application.Devices.Commands.CreateRainbowEffect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firelink.Application.Player.Commands.Listening;

public record SetPlayerListeningCommand(bool Value) : IRequest
{
    public static readonly SetPlayerListeningCommand Listen = new(true);
    public static readonly SetPlayerListeningCommand Stop = new(false);
}
internal sealed class SetPlayerListeningCommandHandler : IRequestHandler<SetPlayerListeningCommand>
{
    private readonly IPlayerListenerService _playerListenerService;

    public SetPlayerListeningCommandHandler(IPlayerListenerService playerListenerService)
    {
        _playerListenerService = playerListenerService;
    }

    public Task Handle(SetPlayerListeningCommand request, CancellationToken cancellationToken)
    {
        _playerListenerService.SetListen(request.Value);
        return Task.CompletedTask;
    }
}