namespace Firelink.Application.Common.Interfaces;

public interface IPlayerListenerService
{
    public bool ShouldListen();

    public void ToggleListen();
}