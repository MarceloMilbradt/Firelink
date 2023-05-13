using System.Drawing;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;

namespace Firelink.App.Server.Features.Spotify;

public class SpotifyApi : ISpotifyConnection
{
    private readonly string _url;
    private readonly string _clientId;
    private readonly string _clientSecret;
    public SpotifyClient? Client { private set; get; }

    public SpotifyApi(IConfiguration configuration)
    {
        _url = configuration.GetValue<string>("Spotify:RedirectUrl")!;
        _clientId = configuration.GetValue<string>("Spotify:ClientId")!;
        _clientSecret = configuration.GetValue<string>("Spotify:ClientSecret")!;
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
}
public interface ISpotifyConnection : ISpotifyAPI
{
    public Task<bool> Connect(string code);
}