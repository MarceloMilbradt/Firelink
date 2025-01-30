using Firelink.Application.Common.Interfaces;
using Firelink.Application.Tracks.Events.TrackChanged;
using Firelink.Application.Tracks.Queries.GetCurrentTrack;
using Firelink.Domain;
using Mediator;

namespace Firelink.Presentation.Workers;

public class PlayerListener : BackgroundService
{
    private readonly PeriodicTimer _timerWhileListening = new(TimeSpan.FromSeconds(1));
    private readonly PeriodicTimer _timerWhileListeningForAWhile = new(TimeSpan.FromSeconds(5));
    private readonly PeriodicTimer _timerWhileWaiting = new(TimeSpan.FromSeconds(30));
    private PeriodicTimer _timer;
    private int _countNumberOfTimesSinceTrackChanged = 0;

    private readonly ILogger<PlayerListener> _logger;
    private readonly ISpotifyApi _spotifyApi;
    private readonly IMediator _mediator;
    private string _currentAlbumId = string.Empty;
    private TrackDto? _currentTrack;

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
        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                await HandleTrack(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while listening");
        }
    }

    private async ValueTask HandleTrack(CancellationToken stoppingToken)
    {
        try
        {
            if (!_spotifyApi.IsUserLoggedIn())
            {
                _logger.LogInformation("Waiting, logIn: {logIn}", _spotifyApi.IsUserLoggedIn());
                _currentAlbumId = string.Empty;
                _timer = _timerWhileWaiting;
                return;
            }

            var currentlyPlaying = await _mediator.Send(GetCurrentTrackQuery.Default, stoppingToken);

            currentlyPlaying.Switch(
                async trackPlaying =>
                {
                    await HandleWhenTrackIsPlaying(trackPlaying, stoppingToken);
                }, noTrack =>
                {
                    HandleNoTrackIsPlaying();
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while listening: {Track} {AlbumId}, {Exception}", _currentTrack?.Name, _currentAlbumId, ex.Message);
        }
    }

    private void HandleNoTrackIsPlaying()
    {
        _timer = _timerWhileWaiting;
        if (_currentAlbumId != string.Empty)
        {
            _logger.LogInformation("Now listening to {Track}", "Nothing");
            _currentAlbumId = string.Empty;
        }
    }

    private async Task HandleWhenTrackIsPlaying(TrackDto currentlyPlaying, CancellationToken stoppingToken)
    {
        _timer = _timerWhileListening;
        if (_currentTrack?.Id != currentlyPlaying.Id || _currentTrack == null)
        {
            _countNumberOfTimesSinceTrackChanged = 0;
            _logger.LogInformation("Now listening to {Track}", currentlyPlaying.Name);
            _currentTrack = currentlyPlaying;
            var trackEvent = new TrackChangedNotification(_currentTrack);
            await _mediator.Publish(trackEvent, stoppingToken);
        }
        else
        {
            _countNumberOfTimesSinceTrackChanged++;
            if (_countNumberOfTimesSinceTrackChanged > 15)
            {
                _timer = _timerWhileListeningForAWhile;
            }
        }
    }
}