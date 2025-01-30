using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Firelink.Application.WLed.Queries.Effects;

public sealed record GetEffectQuery(int EffectId) : IRequest<OneOf<string, None>>;

public sealed class GetEffectQueryHandler : IRequestHandler<GetEffectQuery, OneOf<string, None>>
{

    private readonly IWledService _wledService;

    public GetEffectQueryHandler(IWledService wledService)
    {
        _wledService = wledService;
    }

    public async ValueTask<OneOf<string, None>> Handle(GetEffectQuery request, CancellationToken cancellationToken)
    {
        var effects = await _wledService.GetEffects(cancellationToken);
        var element = effects.ElementAtOrDefault(request.EffectId);
        if (element == null)
        {
            return new None();
        }
        return element;
    }
}