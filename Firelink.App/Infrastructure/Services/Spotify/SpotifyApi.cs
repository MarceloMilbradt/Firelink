using Firelink.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;

namespace Firelink.Infrastructure.Services.Spotify;

public class SpotifyApi : ISpotifyApi
{
    private readonly string _url;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private bool _isLoggedIn;
    public SpotifyClient? Client { private set; get; }
    public SpotifyApi(IConfiguration configuration)
    {
        var spotifyConfig = configuration.GetSection("Spotify");
        _url = spotifyConfig["RedirectUrl"]!;
        _clientId = spotifyConfig["ClientId"]!;
        _clientSecret = spotifyConfig["ClientSecret"]!;
    }
    public async Task<FullTrack?> GetCurrentTrack(CancellationToken cancellationToken)
    {
        if (Client is null)
        {
            return null;
        }
        
        var currentlyPlaying = await Client.Player.GetCurrentlyPlaying(CreateRequest(), cancellationToken);
        if (currentlyPlaying == null)
        {
            return null;
        }
    
        var track = (FullTrack)currentlyPlaying.Item;
        return track;
    }
    private static PlayerCurrentlyPlayingRequest CreateRequest()
    {
        return new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
    }

    public Uri GetLoginUri()
    {
        var loginRequest = new LoginRequest(new Uri(_url), _clientId, LoginRequest.ResponseType.Code)
        {
            Scope = new[] { Scopes.UserReadCurrentlyPlaying, Scopes.UserReadPrivate }
        };
        var uri = loginRequest.ToUri();
        return uri;
    }

    public async Task Connect(string code, CancellationToken cancellationToken)
    {
        if (Client != null)
            return;

        Uri uri = new(_url);
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, uri),
            cancellationToken
        );
        LogIn(response);
    }

    private void LogIn(AuthorizationCodeTokenResponse response)
    {
        var config = SpotifyClientConfig.CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(_clientId, _clientSecret, response));

        Client = new SpotifyClient(config);
        _isLoggedIn = true;
    }

    public bool IsUserLoggedIn()
    {
       return _isLoggedIn;
    }
}