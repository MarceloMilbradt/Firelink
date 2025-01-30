using Firelink.Application.Common.Interfaces;
using Firelink.Domain.CustomEffects;
using Mediator;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Firelink.Application.CustomEffects.Commands.CreateCustomEffect;

public sealed record UpdateDefaultEffectCommand(int PaletteId, int PresetId, int EffectId) : IRequest<OneOf<Success, Error>>;

public sealed class UpdateDefaultEffectCommandHandler : IRequestHandler<UpdateDefaultEffectCommand, OneOf<Success, Error>>
{
    private readonly IWledCustomEffectProvider _wledConfigurationProvider;
    private readonly ILogger<UpdateDefaultEffectCommandHandler> _logger;
    public UpdateDefaultEffectCommandHandler(IWledCustomEffectProvider wledConfigurationProvider, ILogger<UpdateDefaultEffectCommandHandler> logger)
    {
        _wledConfigurationProvider = wledConfigurationProvider;
        _logger = logger;
    }

    public async ValueTask<OneOf<Success, Error>> Handle(UpdateDefaultEffectCommand request, CancellationToken cancellationToken)
    {
        var configurationType = request switch
        {
            { PresetId: > 0 } => ConfigurationType.Preset,
            { EffectId: > 0 } => ConfigurationType.Effect,
            _ => throw new NotImplementedException(),
        };


        var configuration = new CustomEffect
        {
            Title = "Default",
            EffectType = configurationType,
            ItemId = "Default",
            EffectId = request.EffectId,
            PaletteId = request.PaletteId,
            PresetId = request.PresetId,
        };

        try
        {
            await _wledConfigurationProvider.AddCustomEffect(configuration, cancellationToken);
            return new Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Updating Default Effect");
            return new Error();
        }
    }
}
