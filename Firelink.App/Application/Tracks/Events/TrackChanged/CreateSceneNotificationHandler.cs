using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using MediatR;
using TuyaConnector.Data;

namespace Firelink.Application.Tracks.Events.TrackChanged;

internal sealed class CreateSceneNotificationHandler : INotificationHandler<TrackChangedNotification>
{
    private readonly ISpotifyTrackAnalyticsService _trackAnalyticsService;
    private readonly ITuyaConnector _tuyaConnector;

    public CreateSceneNotificationHandler(ISpotifyTrackAnalyticsService trackAnalyticsService,
        ITuyaConnector tuyaConnector)
    {
        _trackAnalyticsService = trackAnalyticsService;
        _tuyaConnector = tuyaConnector;
    }

    public async Task Handle(TrackChangedNotification notification, CancellationToken cancellationToken)
    {
        var track = await _trackAnalyticsService.GetTrackWithFeatures(cancellationToken);
        var energy = track.Energy;

        // Energy value is between 0 and 1, so we scale it to 0-100.
        int unitDuration = (int)(energy * 100);

        var baseHsvSceneColor = new SceneUnit("gradient", unitDuration, track.HsvColor);

        // Determine the number of units based on the energy level.
        int numUnits = unitDuration switch
        {
            <= 50 => 3,
            _ => 2,
        };

        List<SceneUnit> units = new List<SceneUnit>();
        for (int i = 0; i < numUnits; i++)
        {
            var unitSceneColor = baseHsvSceneColor with
            {
                V = baseHsvSceneColor.V * (i + 1) / numUnits // Double V value for each unit
            };

            units.Add(unitSceneColor);
        }


        var scene = new Scene
        {
            SceneNum = 101,
            SceneUnits = units.ToArray()
        };

        var command = new Command
        {
            Code = LedCommands.Scene,
            Value = scene,
        };

        await _tuyaConnector.SendCommandToAllDevices(command, WorkMode.Scene, cancellationToken);

    }

}