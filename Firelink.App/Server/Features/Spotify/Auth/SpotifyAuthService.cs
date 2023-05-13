using SpotifyAPI.Web;

namespace Firelink.App.Server.Features.Spotify.Auth;

public class SpotifyAuthService
{
    private readonly ISpotifyAPI _spotify;
    private readonly string? _url;
    private readonly string? _clientId;

    public SpotifyAuthService(ISpotifyAPI spotify, IConfiguration configuration)
    {
        _spotify = spotify;
        _url = configuration.GetValue<string>("Spotify:RedirectUrl");
        _clientId = configuration.GetValue<string>("Spotify:ClientId");
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

    public async Task LogIn(string code)
    {
        await _spotify.Connect(code);
    }

    public async Task<bool> IsLoggedIn()
    {
        try
        {
            var current = await _spotify.Client.UserProfile.Current();
            return current != null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}