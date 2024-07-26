using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Player.Commands.Listening;

public sealed record SetPlayerListeningCommand(bool Value) : IRequest
{
    public static readonly SetPlayerListeningCommand Listen = new(true);
    public static readonly SetPlayerListeningCommand Stop = new(false);
}

public sealed class SetPlayerListeningCommandHandler : IRequestHandler<SetPlayerListeningCommand>
{
    private readonly IPlayerListenerService _playerListenerService;

    public SetPlayerListeningCommandHandler(IPlayerListenerService playerListenerService)
    {
        _playerListenerService = playerListenerService;
    }

    public ValueTask<Unit> Handle(SetPlayerListeningCommand request, CancellationToken cancellationToken)
    {
        _playerListenerService.SetListen(request.Value);
        return Unit.ValueTask;
    }
}