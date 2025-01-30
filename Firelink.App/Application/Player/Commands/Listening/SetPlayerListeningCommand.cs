using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf.Types;

namespace Firelink.Application.Player.Commands.Listening;

public sealed record SetPlayerListeningCommand(bool Value) : IRequest<Success>
{
    public static readonly SetPlayerListeningCommand Listen = new(true);
    public static readonly SetPlayerListeningCommand Stop = new(false);
}

public sealed class SetPlayerListeningCommandHandler : IRequestHandler<SetPlayerListeningCommand, Success>
{
    private readonly IPlayerListenerService _playerListenerService;

    public SetPlayerListeningCommandHandler(IPlayerListenerService playerListenerService)
    {
        _playerListenerService = playerListenerService;
    }

    public ValueTask<Success> Handle(SetPlayerListeningCommand request, CancellationToken cancellationToken)
    {
        _playerListenerService.SetListen(request.Value);
        return ValueTask.FromResult( new Success() );
    }
}