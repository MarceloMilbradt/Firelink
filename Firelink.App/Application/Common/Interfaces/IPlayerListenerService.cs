namespace Firelink.Application.Common.Interfaces;

public interface IPlayerListenerService
{
    void SetListen(bool shouldListen);
    public bool ShouldListen();

    public void ToggleListen();
}