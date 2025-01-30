using Firelink.Domain;
using Firelink.Domain.CustomEffects;

namespace Firelink.Application.Common.Interfaces;

public interface IWledCustomEffectProvider
{
    Task AddCustomEffect(CustomEffect customEffect, CancellationToken cancellation);
    Task<CustomEffect> GetCustomEffect(string id, CancellationToken cancellation);
    Task<IEnumerable<CustomEffect>> GetCustomEffect(CancellationToken cancellation);
    IEnumerable<CustomEffect> GetLoadedCustomEffects();
    Task<CustomEffect> PickEffectForTrack(TrackDto trackDto, CancellationToken cancellation);
    Task RemoveCustomEffect(string id, CancellationToken cancellation);
}