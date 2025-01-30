using Firelink.Application.Common.Interfaces;
using LazyCache;
using NWled.Models;
using NWled.Requests;
using NWLED;
using System.Text.Json;

namespace Firelink.Infrastructure.Services.Wled;

internal class WledService : IWledService
{

    private readonly IWLedClient _wLedClient;
    private readonly IAppCache _cache;
    public WledService(IWLedClient wLedClient, IAppCache cache)
    {
        _wLedClient = wLedClient;
        _cache = cache;
    }

    public Task<State?> GetState(CancellationToken cancellationToken)
    {
        return _wLedClient.GetStateAsync(cancellationToken);
    }

    public Task SetState(State state, CancellationToken cancellationToken)
    {
        return _wLedClient.PostAsync(state, cancellationToken);
    }

    public async Task<bool> TurnOn(CancellationToken cancellationToken)
    {
        await _wLedClient.PostAsync(new StateRequest { On = true }, cancellationToken);
        return true;
    }


    public Task<IEnumerable<string>> GetEffects(CancellationToken cancellationToken)
    {
        return _cache.GetOrAddAsync("Effects", async (key) => await _wLedClient.GetEffectsAsync(cancellationToken));
    }

    public Task<IEnumerable<string>> GetPalettes(CancellationToken cancellationToken)
    {
        return _cache.GetOrAddAsync("Palettes", async (key) => await _wLedClient.GetPalettesAsync(cancellationToken));
    }

}
