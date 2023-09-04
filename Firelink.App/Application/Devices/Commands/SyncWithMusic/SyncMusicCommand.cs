using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuyaConnector.Data;

namespace Firelink.Application.Devices.Commands.SyncWithMusic;

public sealed record SyncMusicCommand : IRequest<bool>;


internal sealed class SyncMusicCommandHandler : IRequestHandler<SyncMusicCommand, bool>
{
    private readonly ITuyaConnector _tuyaConnector;
    private readonly IPlayerListenerService _playerListenerService;
    private readonly ISpotifyTrackAnalyticsService _trackAnalyticsService;
    public SyncMusicCommandHandler(ITuyaConnector tuyaConnector, IPlayerListenerService playerListenerService, ISpotifyTrackAnalyticsService trackAnalyticsService)
    {
        _tuyaConnector = tuyaConnector;
        _playerListenerService = playerListenerService;
        _trackAnalyticsService = trackAnalyticsService;
    }
    public async Task<bool> Handle(SyncMusicCommand request, CancellationToken cancellationToken)
    {
        _playerListenerService.SetListen(true);
        var currentTrack = await _trackAnalyticsService.GetTrackWithFeatures(cancellationToken);

        var devices = await _tuyaConnector.GetUserDevices(cancellationToken);
        var command = new Command
        {
            Code = LedCommands.Color,
            Value = currentTrack.HsvColor,
        };

        var deviceTasks = devices.Select(device => _tuyaConnector.SendCommandToDevice(device.Id, command, cancellationToken)).ToList();
        await Task.WhenAll(deviceTasks);
        return true;
    }
}
