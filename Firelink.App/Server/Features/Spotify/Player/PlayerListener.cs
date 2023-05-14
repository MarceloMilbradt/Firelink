using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Firelink.Application.Tracks.Events.ColorChanged;
using Firelink.Application.Tracks.Events.TrackChanged;
using Firelink.Application.Tracks.Queries.GetCurrentTrack;
using MediatR;

namespace Firelink.App.Server.Features.Spotify.Player;

public class PlayerListener : BackgroundService
{
    private PeriodicTimer _timerWhileListening = new(TimeSpan.FromMilliseconds(500));
    private PeriodicTimer _timerWhileWaiting = new(TimeSpan.FromSeconds(30));
    private PeriodicTimer? _timer;

    private readonly ILogger<PlayerListener> _logger;
    private readonly IPlayerListenerService _playerService;
    private readonly ISpotifyApi _spotifyApi;
    private readonly IMediator _mediator;
    private string _currentAlbumId = string.Empty;
    private TrackDto _currentTrack;

    public PlayerListener(ILogger<PlayerListener> logger,
        IMediator mediator,
        IPlayerListenerService playerService, ISpotifyApi spotifyApi)
    {
        _logger = logger;
        _mediator = mediator;
        _playerService = playerService;
        _spotifyApi = spotifyApi;
        _timer = _timerWhileListening;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!_spotifyApi.IsUserLoggedIn() || !_playerService.ShouldListen())
                {
                    _timer = _timerWhileWaiting;
                    continue;
                }

                var currentlyPlaying = await _mediator.Send(GetCurrentTrackQuery.Default);

                if (currentlyPlaying == null)
                {
                    _timer = _timerWhileWaiting;
                    await HandleNoTrackIsPlaying(stoppingToken);
                }
                else
                {
                    _timer = _timerWhileListening;
                    await HandleWhenTrackIsPlaying(currentlyPlaying, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while listening: {track} {albumId}", _currentTrack, _currentAlbumId);
            }
        }
    }

    private async Task HandleNoTrackIsPlaying(CancellationToken stoppingToken)
    {
        if (_currentAlbumId != string.Empty)
        {
            _logger.LogInformation("Now listening to {track}", "Nothing");
            _currentAlbumId = string.Empty;
        }
    }

    private async Task HandleWhenTrackIsPlaying(TrackDto currentlyPlaying, CancellationToken stoppingToken)
    {
        if (_currentTrack?.Id != currentlyPlaying.Id)
        {
            _logger.LogInformation("Now listening to {track}", currentlyPlaying.Name);
            _currentTrack = currentlyPlaying;
            await _mediator.Publish(new TrackChangedNotification(_currentTrack), stoppingToken);
            if (_currentTrack.Album.Id != _currentAlbumId)
            {
                _currentAlbumId = _currentTrack.Album.Id;
                await _mediator.Publish(new ColorChangedNotification(_currentTrack.HsvColor), stoppingToken);
            }
        }
    }
}