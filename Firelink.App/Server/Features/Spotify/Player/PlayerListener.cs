using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Firelink.Application.Tracks.Events.ColorChanged;
using Firelink.Application.Tracks.Events.TrackChanged;
using Firelink.Application.Tracks.Queries.GetCurrentTrack;
using MediatR;

namespace Firelink.App.Server.Features.Spotify.Player;

public class PlayerListener : BackgroundService
{
    private readonly PeriodicTimer _timerWhileListening = new(TimeSpan.FromSeconds(1));
    private readonly PeriodicTimer _timerWhileListeningForAWhile = new(TimeSpan.FromSeconds(5));
    private readonly PeriodicTimer _timerWhileWaiting = new(TimeSpan.FromSeconds(30));
    private PeriodicTimer? _timer;
    private int _countNumberOfTimesSinceTrackChanged = 0;

    private readonly ILogger<PlayerListener> _logger;
    private readonly ISpotifyApi _spotifyApi;
    private readonly IMediator _mediator;
    private string _currentAlbumId = string.Empty;
    private TrackDto _currentTrack;

    public PlayerListener(ILogger<PlayerListener> logger,
        IMediator mediator, ISpotifyApi spotifyApi)
    {
        _logger = logger;
        _mediator = mediator;
        _spotifyApi = spotifyApi;
        _timer = _timerWhileListening;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                if (!_spotifyApi.IsUserLoggedIn())
                {
                    _logger.LogInformation("Waiting, logIn: {logIn}", _spotifyApi.IsUserLoggedIn());
                    _currentAlbumId = string.Empty;
                    _timer = _timerWhileWaiting;
                    continue;
                }

                var currentlyPlaying = await _mediator.Send(GetCurrentTrackQuery.Default, stoppingToken);

                if (currentlyPlaying == null)
                {
                    _timer = _timerWhileWaiting;
                    HandleNoTrackIsPlaying(stoppingToken);
                }
                else
                {
                    _timer = _timerWhileListening;
                    await HandleWhenTrackIsPlaying(currentlyPlaying, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while listening: {Track} {AlbumId}, {Exception}", _currentTrack.Name, _currentAlbumId, ex.Message);
            }
        }
    }

    private void HandleNoTrackIsPlaying(CancellationToken stoppingToken)
    {
        if (_currentAlbumId != string.Empty)
        {
            _logger.LogInformation("Now listening to {Track}", "Nothing");
            _currentAlbumId = string.Empty;
        }
    }

    private async Task HandleWhenTrackIsPlaying(TrackDto currentlyPlaying, CancellationToken stoppingToken)
    {
        if (_currentTrack?.Id != currentlyPlaying.Id || _currentTrack is null)
        {
            _countNumberOfTimesSinceTrackChanged = 0;
            _logger.LogInformation("Now listening to {Track}", currentlyPlaying.Name);
            _currentTrack = currentlyPlaying;
            await _mediator.Publish(new TrackChangedNotification(_currentTrack), stoppingToken);
            if (_currentTrack.Album.Id != _currentAlbumId)
            {
                _currentAlbumId = _currentTrack.Album.Id;
                await _mediator.Publish(new ColorChangedNotification(_currentTrack.HsvColor), stoppingToken);
            }
        }
        else
        {
            _countNumberOfTimesSinceTrackChanged++;
            if (_countNumberOfTimesSinceTrackChanged > 10)
            {
                _timer = _timerWhileListeningForAWhile;
            }
        }
    }
}