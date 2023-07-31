using TuyaConnector.Data;

namespace Firelink.Application.Common.Interfaces;

public interface ITuyaConnector
{
    public Task<IEnumerable<Device>> GetUserDevices(CancellationToken cancellationToken);
    public Task<Device> GetDevice(string deviceId, CancellationToken cancellationToken);
    public Task SendCommandToDevice(string deviceId, Command command,CancellationToken cancellationToken);
    Task<Device> SendUpdateCommandToDevice(string deviceId, Command command, CancellationToken cancellationToken);
    Task SendCommandToDevice(string deviceId, Command command, WorkMode mode, CancellationToken cancellationToken);
    Task SendCommandToAllDevices(Command command, WorkMode mode, CancellationToken cancellationToken);
    Task SendCommandToAllDevices(Command command, CancellationToken cancellationToken);
}