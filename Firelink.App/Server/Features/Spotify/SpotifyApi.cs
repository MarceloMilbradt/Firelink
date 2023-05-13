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

}

public interface ISpotifyAPI
{
    public SpotifyClient? Client { get; }
    public Task<bool> Connect(string code);
}