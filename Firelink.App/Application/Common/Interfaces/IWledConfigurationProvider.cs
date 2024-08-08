using Firelink.App.Shared.TrackConfiguration;

namespace Firelink.Application.Common.Interfaces;

public interface IWledConfigurationProvider
{
    Task AddConfiguration(ItemConfiguration configuration, CancellationToken cancellation);
    Task<ItemConfiguration> GetConfiguration(string id, CancellationToken cancellation);
}