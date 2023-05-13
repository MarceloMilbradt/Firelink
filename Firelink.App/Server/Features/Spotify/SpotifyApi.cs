using System.Drawing;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;

namespace Firelink.App.Server.Features.Spotify;

public class SpotifyApi : ISpotifyAPI
{
    protected readonly string _url;
    protected readonly string _clientId;
    protected readonly string _clientSecret;
    public SpotifyClient? Client { private set; get; }
    IAppCache cache = new CachingService();
    private AuthorizationCodeTokenResponse _authorizationCodeTokenResponse;

    public SpotifyApi(IConfiguration configuration)
    {
        // _clientId = configuration.spotify.clientId;
        // _clientSecret = configuration.spotify.clientSecret;
        // _authorizationCodeTokenResponse =
            // FileManager.ReadFromJsonFile<AuthorizationCodeTokenResponse>("credentials.json");
        // if (_authorizationCodeTokenResponse != null && !_authorizationCodeTokenResponse.IsExpired)
        // {
        //     LogIn(_authorizationCodeTokenResponse);
        // }
    }

    // private static PlayerCurrentlyPlayingRequest CreateRequest()
    // {
    //     return new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
    // }
    //
    public async Task<bool> Connect(string code)
    {
        if (Client != null)
            return true;

        Uri uri = new(_url);
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, uri)
        );

        LogIn(response);
        return true;
    }

    private void LogIn(AuthorizationCodeTokenResponse response)
    {
        var config = SpotifyClientConfig.CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(_clientId, _clientSecret, response));

        Client = new SpotifyClient(config);
    }

    // public async Task<TrackAudioFeatures?> GetAudioFeatures()
    // {
    //     if (_spotify is null) return null;
    //     var track = await GetCurrentTrack();
    //     if (track is null) return null;
    //     return await GetFeatures(track);
    // }

    // public async Task<TrackAudioFeatures> GetFeatures(FullTrack? track)
    // {
    //     if (track is null) return null;
    //     return await _spotify.Tracks.GetAudioFeatures(track.Id);
    // }

    // public async Task<TrackAudioAnalysis> GetAnalysis(FullTrack? track)
    // {
    //     if (track is null) return null;
    //     return await _spotify.Tracks.GetAudioAnalysis(track.Id);
    // }
    //
    // public async Task<PrivateUser?> GetCurrentUser()
    // {
    //     if (_spotify is null) return null;
    //     var user = await _spotify.UserProfile.Current();
    //     return user;
    // }


    // public async Task<FullTrack?> GetTrack()
    // {
    //     return await GetCurrentTrack();
    // }

    // public async Task<FullTrack?> GetCurrentTrack()
    // {
    //     if (_spotify is null) return null;
    //
    //     var currentlyPlaying = await _spotify.Player.GetCurrentlyPlaying(CreateRequest());
    //     if (currentlyPlaying == null)
    //     {
    //         return null;
    //     }
    //
    //     var track = (FullTrack)currentlyPlaying.Item;
    //     return track;
    // }
    //
    // public async Task<CurrentlyPlaying?> GetCurrent()
    // {
    //     if (_spotify is null) return null;
    //
    //     var currentlyPlaying = await _spotify.Player.GetCurrentlyPlaying(CreateRequest());
    //     if (currentlyPlaying == null)
    //     {
    //         return null;
    //     }
    //
    //     return currentlyPlaying;
    // }
    //
    // public async Task<TrackDTO> GetFullInfo(FullTrack track)
    // {
    //     try
    //     {
    //         return await cache.GetOrAddAsync(track.Id, async entry =>
    //         {
    //             var feturesTask = GetFeatures(track);
    //             var analysisTask = GetAnalysis(track);
    //             var color = ColorScraper.ScrapeColorForAlbum(track.Album.ExternalUrls["spotify"]);
    //             await Task.WhenAll(feturesTask, analysisTask);
    //             var info = new TrackDTO(track, feturesTask.Result, color, analysisTask.Result);
    //             return info;
    //         }, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) });
    //     }
    //     catch (Exception)
    //     {
    //         return new TrackDTO(null, null, Color.Transparent, null);
    //     }
    // }
}

public interface ISpotifyAPI
{
    public SpotifyClient? Client { get; }
    public Task<bool> Connect(string code);
}