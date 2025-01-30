using Firelink.Domain.CustomEffects;
using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf.Types;
using OneOf;

namespace Firelink.Application.CustomEffects.Queries.GetCustomEffect;

public sealed record GetCustomEffectByIdQuery(string Id) : IRequest<OneOf<CustomEffect, None>>;

public sealed class GetCustomEffectByIdQueryHandler : IRequestHandler<GetCustomEffectByIdQuery, OneOf<CustomEffect, None>>
{

    private readonly IWledCustomEffectProvider _wledConfigurationProvider;

    public GetCustomEffectByIdQueryHandler(IWledCustomEffectProvider wledConfigurationProvider)
    {
        _wledConfigurationProvider = wledConfigurationProvider;
    }

    public async ValueTask<OneOf<CustomEffect, None>> Handle(GetCustomEffectByIdQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _wledConfigurationProvider.GetCustomEffect(request.Id, cancellationToken);
        if (configuration == null)
        {
            return new None();
        }

        return configuration;
    }
}

