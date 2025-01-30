using Firelink.Application.Common.Interfaces;
using Firelink.Domain;
using Firelink.Domain.CustomEffects;
using Firelink.Infrastructure.Common.Persistence;
using WledCustomEffectStorage = System.Collections.Generic.Dictionary<string, Firelink.Domain.CustomEffects.CustomEffect>;


namespace Firelink.Infrastructure.Services.Wled;

internal class WledConfigurationProvider : IAsyncDisposable, IWledCustomEffectProvider
{

    private const string JsonFilePath = "CustomEffects";
    private WledCustomEffectStorage _customEffects;

    public async ValueTask DisposeAsync()
    {
        await Save(default);
    }

    public async Task<CustomEffect> GetCustomEffect(string id, CancellationToken cancellation)
    {
        await LoadFromFile(cancellation);

        if (_customEffects.TryGetValue(id, out var item))
        {
            return item;
        }

        return null;
    }

    public async Task RemoveCustomEffect(string id, CancellationToken cancellation)
    {
        await LoadFromFile(cancellation);
        _customEffects.Remove(id);
        await Save(cancellation);
    }

    private async Task Save(CancellationToken cancellation)
    {
        if (_customEffects != null && _customEffects.Where(c => c.Key != "Default").Any())
        {
            await MemoryPackFileManager.SaveToFile(JsonFilePath, _customEffects, cancellation);
        }
    }

    private async ValueTask LoadFromFile(CancellationToken cancellation)
    {
        _customEffects ??= await MemoryPackFileManager.LoadFromFile<WledCustomEffectStorage>(JsonFilePath, cancellation) ?? [];
    }

    public async Task AddCustomEffect(CustomEffect customEffect, CancellationToken cancellation)
    {
        await LoadFromFile(cancellation);
        _customEffects.Remove(customEffect.ItemId);
        _customEffects.Add(customEffect.ItemId, customEffect);

        await Save(cancellation);
    }

    public async Task<IEnumerable<CustomEffect>> GetCustomEffect(CancellationToken cancellation)
    {
        await LoadFromFile(cancellation);
        return _customEffects.Values;
    }

    public IEnumerable<CustomEffect> GetLoadedCustomEffects()
    {
        return _customEffects.Values;
    }

    public async Task<CustomEffect> PickEffectForTrack(TrackDto trackDto, CancellationToken cancellation)
    {
        await LoadFromFile(cancellation);
        if (_customEffects.TryGetValue(trackDto.Id, out CustomEffect effect))
        {
            return effect;
        }
        if (_customEffects.TryGetValue(trackDto.Album.Id, out effect))
        {
            return effect;
        }

        _customEffects.TryGetValue("Default", out effect!);
        return effect;
    }
}


