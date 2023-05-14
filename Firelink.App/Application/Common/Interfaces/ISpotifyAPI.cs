using SpotifyAPI.Web;

namespace Firelink.Application.Common.Interfaces;

public interface ISpotifyApi
{
    SpotifyClient Client { get; }
    Task Connect(string code, CancellationToken cancellationToken);
    Task<FullTrack?> GetCurrentTrack(CancellationToken cancellationToken);
    bool IsUserLoggedIn();
    Uri GetLoginUri();
}