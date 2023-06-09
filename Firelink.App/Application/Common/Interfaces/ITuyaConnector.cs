﻿using TuyaConnector.Data;

namespace Firelink.Application.Common.Interfaces;

public interface ITuyaConnector
{
    public Task<IEnumerable<Device>> GetUserDevices(CancellationToken cancellationToken);
    public Task<Device> GetDevice(string deviceId, CancellationToken cancellationToken);
    public Task SendCommandToDevice(string deviceId, Command command,CancellationToken cancellationToken);
}