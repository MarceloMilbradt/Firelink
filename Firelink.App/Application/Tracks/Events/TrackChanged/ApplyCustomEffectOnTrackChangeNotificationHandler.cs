using Firelink.Application.Common.Configuration;
using Firelink.Application.Common.Interfaces;
using Firelink.Domain.CustomEffects;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace Firelink.Application.Tracks.Events.TrackChanged;

public sealed class ApplyCustomEffectOnTrackChangeNotificationHandler : INotificationHandler<TrackChangedNotification>
{
    private readonly IPlayerListenerService _playerListenerService;
    private readonly IWledService _wledService;
    private readonly IWledCustomEffectProvider _wledCustomEffectProvider;
    private readonly IOptionsMonitor<LedColorConfiguration> _colorOptions;
    private readonly ILogger<ApplyCustomEffectOnTrackChangeNotificationHandler> _logger;
    public ApplyCustomEffectOnTrackChangeNotificationHandler(
        IPlayerListenerService playerListenerService,
        IWledService wledService,
        IWledCustomEffectProvider wledCustomEffectProvider,
        IOptionsMonitor<LedColorConfiguration> colorOptions,
        ILogger<ApplyCustomEffectOnTrackChangeNotificationHandler> logger)
    {
        _playerListenerService = playerListenerService;
        _wledService = wledService;
        _wledCustomEffectProvider = wledCustomEffectProvider;
        _colorOptions = colorOptions;
        _logger = logger;
    }

    public async ValueTask Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            if (!_playerListenerService.ShouldListen()) return;

            var track = notification.Track;

            var effect = await _wledCustomEffectProvider.PickEffectForTrack(track, cancellationToken);

            var state = await _wledService.GetState(cancellationToken);
            if (state == null)
            {
                return;
            }

            var colorConfiguration = _colorOptions.CurrentValue;

            switch (effect.EffectType)
            {
                case ConfigurationType.Preset:
                    state.PresetId = effect.PresetId;
                    break;
                case ConfigurationType.Effect:
                    var seg = state.Segments.ElementAt(state.MainSegment);
                    seg.EffectId = effect.EffectId;
                    seg.ColorPaletteId = effect.PaletteId;
                    state.PresetId = 0;
                    var color = track.Color.ShiftBrightness(colorConfiguration.MainBrightnessFactor);
                    seg.Colors[0] = color.ToRGBAArray();
                    seg.Colors[2] = color.ShiftBrightness(colorConfiguration.SecondaryBrightnessFactor).ToRGBAArray();
                    seg.Colors[1] = color.ShiftBrightness(colorConfiguration.BgBrightnessFactor).ToRGBAArray();
                    break;
            }

            await _wledService.SetState(state, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Applying effect");
        }
    }
}