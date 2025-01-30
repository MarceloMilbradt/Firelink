using Firelink.Domain.CustomEffects;
using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.CustomEffects.Queries.GetCustomEffect;

public sealed record GetAllCustomEffectsQuery() : IRequest<IEnumerable<CustomEffect>>
{
    public static GetAllCustomEffectsQuery Default = new GetAllCustomEffectsQuery();
}

public sealed class GetAllCustomEffectsQueryHandler : IRequestHandler<GetAllCustomEffectsQuery, IEnumerable<CustomEffect>>
{
    private readonly IWledCustomEffectProvider _wledConfigurationProvider;

    public GetAllCustomEffectsQueryHandler(IWledCustomEffectProvider wledConfigurationProvider)
    {
        _wledConfigurationProvider = wledConfigurationProvider;
    }

    public async ValueTask<IEnumerable<CustomEffect>> Handle(GetAllCustomEffectsQuery request, CancellationToken cancellationToken)
    {
        return await _wledConfigurationProvider.GetCustomEffect(cancellationToken);
    }
}
