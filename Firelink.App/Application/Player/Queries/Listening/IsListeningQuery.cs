using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Player.Queries.Listening;

public record IsListeningQuery : IRequest<bool>;
public sealed class IsListeningQueryHandler : IRequestHandler<IsListeningQuery, bool>
{
    private readonly IPlayerListenerService _playerListenerService;

    public IsListeningQueryHandler(IPlayerListenerService playerListenerService)
    {
        _playerListenerService = playerListenerService;
    }

    public ValueTask<bool> Handle(IsListeningQuery request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_playerListenerService.ShouldListen());
    }
}