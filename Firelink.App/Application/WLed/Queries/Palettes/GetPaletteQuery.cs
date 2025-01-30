using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf.Types;
using OneOf;

namespace Firelink.Application.WLed.Queries.Palettes;


public sealed record GetPaletteQuery(int PaletteId) : IRequest<OneOf<string, None>>;

public sealed class GetPaletteQueryHandler : IRequestHandler<GetPaletteQuery, OneOf<string, None>>
{

    private readonly IWledService _wledService;

    public GetPaletteQueryHandler(IWledService wledService)
    {
        _wledService = wledService;
    }

    public async ValueTask<OneOf<string, None>> Handle(GetPaletteQuery request, CancellationToken cancellationToken)
    {
        var palettes = await _wledService.GetPalettes(cancellationToken);
        var element = palettes.ElementAtOrDefault(request.PaletteId);
        if (element == null)
        {
            return new None();
        }
        return element;
    }
}