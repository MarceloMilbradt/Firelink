using Firelink.Application.Common.Interfaces;

namespace Firelink.Infrastructure.Services.Spotify;

public class PlayerListenerService : IPlayerListenerService
{
    private bool _shouldListen = true;
    public bool ShouldListen()
    {
        return _shouldListen;
    }

    public void ToggleListen()
    {
        _shouldListen = !_shouldListen;
    }

    public void SetListen(bool shouldListen)
    {
        _shouldListen = shouldListen;
    }
}