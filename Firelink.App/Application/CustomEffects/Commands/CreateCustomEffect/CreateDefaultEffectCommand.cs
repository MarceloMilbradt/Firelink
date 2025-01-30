using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Result;
using Firelink.Domain.CustomEffects;
using Mediator;
using OneOf.Types;
using OneOf;
using Microsoft.Extensions.Logging;

namespace Firelink.Application.CustomEffects.Commands.CreateCustomEffect;

public sealed record CreateDefaultEffectCommand(int PaletteId, int PresetId, int EffectId, string Json) : IRequest<OneOf<Success, Error>>;

public sealed class CreateDefaultEffectCommandHandler : IRequestHandler<CreateDefaultEffectCommand, OneOf<Success, Error>>
{
    private readonly IWledCustomEffectProvider _wledConfigurationProvider;
    private readonly ILogger<CreateDefaultEffectCommandHandler> _logger;
    public CreateDefaultEffectCommandHandler(IWledCustomEffectProvider wledConfigurationProvider, ILogger<CreateDefaultEffectCommandHandler> logger)
    {
        _wledConfigurationProvider = wledConfigurationProvider;
        _logger = logger;
    }

    public async ValueTask<OneOf<Success, Error>> Handle(CreateDefaultEffectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingConfiguration = await _wledConfigurationProvider.GetCustomEffect("Default", cancellationToken);
            if (existingConfiguration != null)
            {
                return new Success();
            }

            var configurationType = request switch
            {
                { PresetId: > 0 } => ConfigurationType.Preset,
                { EffectId: > 0 } => ConfigurationType.Effect,
                _ => throw new InvalidOperationException()
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

            await _wledConfigurationProvider.AddCustomEffect(configuration, cancellationToken);
            return new Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating default effect");
            return new Error();
        }
    }
}
