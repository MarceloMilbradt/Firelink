using Firelink.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firelink.Application.Player.Queries.Listening;

public record IsListeningQuery : IRequest<bool>;
internal sealed class IsListeningQueryHandler : IRequestHandler<IsListeningQuery, bool>
{
    private readonly IPlayerListenerService _playerListenerService;

    public IsListeningQueryHandler(IPlayerListenerService playerListenerService)
    {
        _playerListenerService = playerListenerService;
    }

    public Task<bool> Handle(IsListeningQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_playerListenerService.ShouldListen());
    }
}