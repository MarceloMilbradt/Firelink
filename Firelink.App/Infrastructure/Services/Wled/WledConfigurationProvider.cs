using Firelink.App.Shared.TrackConfiguration;
using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Common.Persistence;

namespace Firelink.Infrastructure.Services.Wled;

internal class WledConfigurationProvider : IAsyncDisposable, IWledConfigurationProvider
{

    private const string JsonFilePath = "WledConfiguration.json";
    private Dictionary<string, ItemConfiguration> _configurations;

    public async ValueTask DisposeAsync()
    {
        await JsonFileManager.SaveToJson(JsonFilePath, _configurations, default);
    }

    public async Task<ItemConfiguration> GetConfiguration(string id, CancellationToken cancellation)
    {
        _configurations ??= await JsonFileManager.LoadFromJson<Dictionary<string, ItemConfiguration>>(JsonFilePath, cancellation) ?? [];

        if (_configurations.TryGetValue(id, out var item))
        {
            return item;
        }

        return null;
    }


    public async Task AddConfiguration(ItemConfiguration configuration, CancellationToken cancellation)
    {
        _configurations.Add(configuration.ItemId, configuration);
        await JsonFileManager.SaveToJson(JsonFilePath, _configurations, default);
    }
}
